using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Data;
using Microsoft.EntityFrameworkCore;

namespace KooperatifYonetim.Business.Services
{
    public class UrunTeminService : IUrunTeminService
    {
        private readonly AppDbContext _db;

        public UrunTeminService(AppDbContext db) => _db = db;

        public async Task<List<UrunTemin>> GetListeAsync(string userId, bool isAdmin, bool isToptanci)
        {
            var query = _db.UrunTeminler
                .Include(u => u.Arazi).ThenInclude(a => a.Uretici)
                .Include(u => u.Toptanci)
                .AsQueryable();

            if (!isAdmin)
            {
                if (isToptanci)
                    query = query.Where(u => u.ToptanciId == userId);
                else
                    query = query.Where(u => u.Arazi.UreticiId == userId);
            }

            return await query.OrderByDescending(u => u.Donem).ToListAsync();
        }

        public async Task<UrunTemin?> GetByIdAsync(int id)
            => await _db.UrunTeminler
                .Include(u => u.Arazi).ThenInclude(a => a.Uretici)
                .Include(u => u.Toptanci)
                .FirstOrDefaultAsync(u => u.UrunTeminId == id);

        public async Task<int> CreateAsync(UrunTemin temin)
        {
            _db.UrunTeminler.Add(temin);
            await _db.SaveChangesAsync();
            return temin.UrunTeminId;
        }

        public async Task UpdateAsync(UrunTemin temin)
        {
            _db.UrunTeminler.Update(temin);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var temin = await _db.UrunTeminler.FindAsync(id);
            if (temin is not null)
            {
                _db.UrunTeminler.Remove(temin);
                await _db.SaveChangesAsync();
            }
        }
    }
}
