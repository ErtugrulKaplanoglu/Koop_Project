using KooperatifYonetim.Core.Entities;

namespace KooperatifYonetim.Core.Interfaces
{
    public interface IGunlukBesiGirisiService
    {
        Task<IEnumerable<GunlukBesiGirisi>> GetListeAsync(string userId, bool isAdmin);
        Task CreateAsync(GunlukBesiGirisi giris);
    }
}
