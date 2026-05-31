using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;

namespace KooperatifYonetim.Business.Services
{
    public class GunlukBesiGirisiService : IGunlukBesiGirisiService
    {
        public Task<IEnumerable<GunlukBesiGirisi>> GetListeAsync(string userId, bool isAdmin)
            => Task.FromResult(Enumerable.Empty<GunlukBesiGirisi>());

        public Task CreateAsync(GunlukBesiGirisi giris) => Task.CompletedTask;
    }
}
