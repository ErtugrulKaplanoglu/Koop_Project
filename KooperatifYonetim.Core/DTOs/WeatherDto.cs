namespace KooperatifYonetim.Core.DTOs
{
    public class WeatherDto
    {
        public double Sicaklik { get; set; }
        public string Durum { get; set; } = string.Empty;
        public string IkonKodu { get; set; } = string.Empty;
        public int Nem { get; set; }
        public double RuzgarHizi { get; set; }
        public string SehirAdi { get; set; } = string.Empty;
        public string IkonUrl => $"https://openweathermap.org/img/wn/{IkonKodu}@2x.png";
    }
}
