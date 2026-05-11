using KooperatifYonetim.Core.DTOs;

namespace KooperatifYonetim.Core.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDataDto> GetYoneticiAsync();
        Task<DashboardDataDto> GetUreticiAsync(string userId);
        Task<DashboardDataDto> GetZiraatMuhendisiAsync(string userId);
        Task<DashboardDataDto> GetVeterinerAsync(string userId);
        Task<DashboardDataDto> GetMandiraAsync(string userId);
        Task<DashboardDataDto> GetToptanciAsync(string userId);
        Task<DashboardDataDto> GetTedarikciAsync(string userId);
    }
}
