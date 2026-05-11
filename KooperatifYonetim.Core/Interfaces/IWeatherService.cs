using KooperatifYonetim.Core.DTOs;

namespace KooperatifYonetim.Core.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherDto?> GetWeatherAsync(double lat, double lon);
    }
}
