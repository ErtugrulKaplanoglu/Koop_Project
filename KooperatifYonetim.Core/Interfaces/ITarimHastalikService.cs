using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Enums;

namespace KooperatifYonetim.Core.Interfaces
{
    public interface ITarimHastalikService
    {
        Task<List<TarimHastalikBildirimi>> GetListeAsync(string userId, string rolAdi);
        Task<TarimHastalikBildirimi?> GetByIdAsync(int id);
        Task<int> CreateAsync(TarimHastalikBildirimi bildirim);
        Task DurumGuncelleAsync(int id, BildirimDurum yeniDurum, string muhendisId);
    }
}
