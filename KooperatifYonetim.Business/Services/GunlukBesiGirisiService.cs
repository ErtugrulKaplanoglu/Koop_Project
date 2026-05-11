using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Data;
using Microsoft.EntityFrameworkCore;

namespace KooperatifYonetim.Business.Services
{
    public class GunlukBesiGirisiService : IGunlukBesiGirisiService
    {
        private readonly AppDbContext _db;
        public GunlukBesiGirisiService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<GunlukBesiGirisi>> GetListeAsync(string userId, bool isAdmin)
        {
            var query = _db.GunlukBesiGirisleri
                .Include(g => g.Ahir).ThenInclude(a => a.Uretici)
                .AsQueryable();
            if (!isAdmin)
                query = query.Where(g => g.Ahir.UreticiId == userId);
            return await query.OrderByDescending(g => g.Tarih).Take(100).ToListAsync();
        }

        public async Task CreateAsync(GunlukBesiGirisi giris)
        {
            _db.GunlukBesiGirisleri.Add(giris);
            await _db.SaveChangesAsync();
        }
    }
}
