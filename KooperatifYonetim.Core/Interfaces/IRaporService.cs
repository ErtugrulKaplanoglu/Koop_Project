using KooperatifYonetim.Core.DTOs;

namespace KooperatifYonetim.Core.Interfaces
{
    public interface IRaporService
    {
        Task<UreticiAylikOzetDto> GetUreticiAylikOzetAsync(string ureticiId, int yil, int ay);
        Task<List<YoneticiUreticiSatirDto>> GetYoneticiAylikOzetAsync(int yil, int ay);
        Task<List<GunlukSutDto>> GetMandiraAylikSutAsync(string mandiraId, int yil, int ay);
    }
}
