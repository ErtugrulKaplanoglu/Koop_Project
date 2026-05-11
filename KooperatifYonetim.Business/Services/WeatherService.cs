using KooperatifYonetim.Core.DTOs;
using KooperatifYonetim.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace KooperatifYonetim.Business.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _http;
        private readonly IMemoryCache _cache;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public WeatherService(HttpClient http, IMemoryCache cache, IConfiguration config)
        {
            _http = http;
            _cache = cache;
            _apiKey = config["OpenWeatherMap:ApiKey"] ?? string.Empty;
            _baseUrl = config["OpenWeatherMap:BaseUrl"] ?? "https://api.openweathermap.org/data/2.5";
        }

        public async Task<WeatherDto?> GetWeatherAsync(double lat, double lon)
        {
            if (string.IsNullOrWhiteSpace(_apiKey) || _apiKey == "YOUR_API_KEY_HERE")
                return null;

            var cacheKey = $"weather_{lat:F3}_{lon:F3}_{DateTime.Today:yyyyMMdd}";
            if (_cache.TryGetValue(cacheKey, out WeatherDto? cached))
                return cached;

            try
            {
                var url = $"{_baseUrl}/weather?lat={lat.ToString(System.Globalization.CultureInfo.InvariantCulture)}&lon={lon.ToString(System.Globalization.CultureInfo.InvariantCulture)}&appid={_apiKey}&units=metric&lang=tr";
                var response = await _http.GetStringAsync(url);
                var json = JObject.Parse(response);

                var dto = new WeatherDto
                {
                    Sicaklik = json["main"]?["temp"]?.Value<double>() ?? 0,
                    Nem = json["main"]?["humidity"]?.Value<int>() ?? 0,
                    RuzgarHizi = json["wind"]?["speed"]?.Value<double>() ?? 0,
                    Durum = json["weather"]?[0]?["description"]?.Value<string>() ?? string.Empty,
                    IkonKodu = json["weather"]?[0]?["icon"]?.Value<string>() ?? "01d",
                    SehirAdi = json["name"]?.Value<string>() ?? string.Empty
                };

                // Aynı gün için cache'le
                _cache.Set(cacheKey, dto, new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Today.AddDays(1)
                });

                return dto;
            }
            catch
            {
                return null;
            }
        }
    }
}
