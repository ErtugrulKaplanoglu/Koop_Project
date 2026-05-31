using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Enums;

namespace KooperatifYonetim.Core.Interfaces
{
    public interface IYemTedarikService
    {
        Task<IEnumerable<YemTedarikBasvuru>> GetListeAsync(string userId, bool isAdmin);
        Task<YemTedarikBasvuru?> GetByIdAsync(int id);
        Task CreateAsync(YemTedarikBasvuru basvuru, IEnumerable<string> yoneticiIdleri);
        Task DurumGuncelleAsync(int id, BasvuruDurum yeniDurum, string? yoneticiNotu);
    }
}
