using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Enums;

namespace KooperatifYonetim.Core.Interfaces
{
    public interface IHayvanHastalikService
    {
        Task<IEnumerable<HayvanHastalikBildirimi>> GetListeAsync(string userId, string rolAdi);
        Task<HayvanHastalikBildirimi?> GetByIdAsync(int id);
        Task CreateAsync(HayvanHastalikBildirimi bildirim);
        Task DurumGuncelleAsync(int id, BildirimDurum yeniDurum, string veterinerId);
    }
}
