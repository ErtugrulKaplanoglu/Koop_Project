using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Data;
using Microsoft.EntityFrameworkCore;

namespace KooperatifYonetim.Business.Services
{
    public class AraziService : IAraziService
    {
        private readonly AppDbContext _db;

        public AraziService(AppDbContext db) => _db = db;

        public async Task<List<Arazi>> GetListeAsync(string userId, bool isAdmin)
        {
            var query = _db.Araziler
                .Include(a => a.Uretici)
                .Include(a => a.Ekinler)
                .AsQueryable();

            if (!isAdmin)
                query = query.Where(a => a.UreticiId == userId);

            return await query.OrderBy(a => a.Ad).ToListAsync();
        }

        public async Task<Arazi?> GetByIdAsync(int id)
            => await _db.Araziler.Include(a => a.Uretici).FirstOrDefaultAsync(a => a.AraziId == id);

        public async Task<Arazi?> GetDetayAsync(int id)
            => await _db.Araziler
                .Include(a => a.Uretici)
                .Include(a => a.Ekinler)
                .Include(a => a.UrunTeminler).ThenInclude(u => u.Toptanci)
                .FirstOrDefaultAsync(a => a.AraziId == id);

        public async Task<int> CreateAsync(Arazi arazi)
        {
            _db.Araziler.Add(arazi);
            await _db.SaveChangesAsync();
            return arazi.AraziId;
        }

        public async Task UpdateAsync(Arazi arazi)
        {
            _db.Araziler.Update(arazi);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var arazi = await _db.Araziler.FindAsync(id);
            if (arazi is not null)
            {
                _db.Araziler.Remove(arazi);
                await _db.SaveChangesAsync();
            }
        }
    }
}
