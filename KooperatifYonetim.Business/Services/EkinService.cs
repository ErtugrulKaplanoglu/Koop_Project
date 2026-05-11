using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Data;
using Microsoft.EntityFrameworkCore;

namespace KooperatifYonetim.Business.Services
{
    public class EkinService : IEkinService
    {
        private readonly AppDbContext _db;

        public EkinService(AppDbContext db) => _db = db;

        public async Task<List<Ekin>> GetListeAsync(string userId, bool isAdmin)
        {
            var query = _db.Ekinler
                .Include(e => e.Arazi).ThenInclude(a => a.Uretici)
                .AsQueryable();

            if (!isAdmin)
                query = query.Where(e => e.Arazi.UreticiId == userId);

            return await query.OrderByDescending(e => e.EkimTarihi).ToListAsync();
        }

        public async Task<List<Ekin>> GetByAraziAsync(int araziId)
            => await _db.Ekinler
                .Where(e => e.AraziId == araziId)
                .OrderByDescending(e => e.EkimTarihi)
                .ToListAsync();

        public async Task<Ekin?> GetByIdAsync(int id)
            => await _db.Ekinler.Include(e => e.Arazi).FirstOrDefaultAsync(e => e.EkinId == id);

        public async Task<Ekin?> GetDetayAsync(int id)
            => await _db.Ekinler
                .Include(e => e.Arazi).ThenInclude(a => a.Uretici)
                .Include(e => e.TarimIslemler)
                .Include(e => e.HastalıkBildirimler).ThenInclude(b => b.Uretici)
                .FirstOrDefaultAsync(e => e.EkinId == id);

        public async Task<int> CreateAsync(Ekin ekin)
        {
            _db.Ekinler.Add(ekin);
            await _db.SaveChangesAsync();
            return ekin.EkinId;
        }

        public async Task UpdateAsync(Ekin ekin)
        {
            _db.Ekinler.Update(ekin);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ekin = await _db.Ekinler.FindAsync(id);
            if (ekin is not null)
            {
                _db.Ekinler.Remove(ekin);
                await _db.SaveChangesAsync();
            }
        }
    }
}
