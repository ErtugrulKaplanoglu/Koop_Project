using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Data;
using Microsoft.EntityFrameworkCore;

namespace KooperatifYonetim.Business.Services
{
    public class BesiStokService : IBesiStokService
    {
        private readonly AppDbContext _db;
        public BesiStokService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<BesiStok>> GetListeAsync(string userId, bool isAdmin)
        {
            var query = _db.BesiStoklar
                .Include(b => b.Ahir).ThenInclude(a => a.Uretici)
                .AsQueryable();
            if (!isAdmin)
                query = query.Where(b => b.Ahir.UreticiId == userId);
            return await query.OrderBy(b => b.Ahir.Ad).ThenBy(b => b.BesiTuru).ToListAsync();
        }

        public async Task<BesiStok?> GetByIdAsync(int id) =>
            await _db.BesiStoklar.Include(b => b.Ahir).FirstOrDefaultAsync(b => b.StokId == id);

        public async Task CreateAsync(BesiStok stok)
        {
            stok.SonGuncelleme = DateTime.Now;
            _db.BesiStoklar.Add(stok);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(BesiStok stok)
        {
            stok.SonGuncelleme = DateTime.Now;
            _db.BesiStoklar.Update(stok);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var stok = await _db.BesiStoklar.FindAsync(id);
            if (stok is null) return;
            _db.BesiStoklar.Remove(stok);
            await _db.SaveChangesAsync();
        }
    }
}
