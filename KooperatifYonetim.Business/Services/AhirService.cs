using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Data;
using Microsoft.EntityFrameworkCore;

namespace KooperatifYonetim.Business.Services
{
    public class AhirService : IAhirService
    {
        private readonly AppDbContext _db;
        public AhirService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Ahir>> GetListeAsync(string userId, bool isAdmin)
        {
            var query = _db.Ahirlar.Include(a => a.Uretici).AsQueryable();
            if (!isAdmin) query = query.Where(a => a.UreticiId == userId);
            return await query.OrderBy(a => a.Ad).ToListAsync();
        }

        public async Task<Ahir?> GetByIdAsync(int id) =>
            await _db.Ahirlar
                .Include(a => a.Uretici)
                .Include(a => a.BesiStoklar)
                .Include(a => a.BesiHareketleri)
                .Include(a => a.SutUretimler).ThenInclude(s => s.Mandira)
                .Include(a => a.VeterinerBakimlar).ThenInclude(v => v.Veteriner)
                .Include(a => a.HayvanHastalikBildirimler).ThenInclude(h => h.Veteriner)
                .FirstOrDefaultAsync(a => a.AhirId == id);

        public async Task CreateAsync(Ahir ahir)
        {
            _db.Ahirlar.Add(ahir);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ahir ahir)
        {
            _db.Ahirlar.Update(ahir);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ahir = await _db.Ahirlar.FindAsync(id);
            if (ahir is null) return;
            ahir.AktifMi = false;
            await _db.SaveChangesAsync();
        }
    }
}
