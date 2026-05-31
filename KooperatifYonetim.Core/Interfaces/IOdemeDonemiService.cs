using KooperatifYonetim.Core.Entities;

namespace KooperatifYonetim.Core.Interfaces
{
    public interface IOdemeDonemiService
    {
        Task<List<OdemeDonemi>> GetListeAsync();
        Task<OdemeDonemi?> GetByIdAsync(int id);
        Task<OdemeDonemi?> GetDetayAsync(int id);
        Task<int> CreateAsync(int yil, int ay);
        Task HesaplaAsync(int odemeDonemiId);
        Task OnaylaAsync(int odemeDonemiId);
        Task<List<UreticiOdeme>> GetUreticiGecmisAsync(string ureticiId);
    }
}
