using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Enums;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Data;
using Microsoft.EntityFrameworkCore;

namespace KooperatifYonetim.Business.Services
{
    public class TarimIslemService : ITarimIslemService
    {
        private readonly AppDbContext _db;

        public TarimIslemService(AppDbContext db) => _db = db;

        public async Task<List<TarimIslem>> GetListeAsync(string userId, bool isAdmin)
        {
            var query = _db.TarimIslemler
                .Include(t => t.Ekin).ThenInclude(e => e.Arazi).ThenInclude(a => a.Uretici)
                .AsQueryable();

            if (!isAdmin)
                query = query.Where(t => t.Ekin.Arazi.UreticiId == userId);

            return await query.OrderBy(t => t.PlanlananTarih).ToListAsync();
        }

        public async Task<List<TarimIslem>> GetByEkinAsync(int ekinId)
            => await _db.TarimIslemler
                .Where(t => t.EkinId == ekinId)
                .OrderBy(t => t.PlanlananTarih)
                .ToListAsync();

        public async Task<TarimIslem?> GetByIdAsync(int id)
            => await _db.TarimIslemler
                .Include(t => t.Ekin).ThenInclude(e => e.Arazi)
                .FirstOrDefaultAsync(t => t.IslemId == id);

        public async Task<int> CreateAsync(TarimIslem islem)
        {
            _db.TarimIslemler.Add(islem);
            await _db.SaveChangesAsync();
            return islem.IslemId;
        }

        public async Task UpdateAsync(TarimIslem islem)
        {
            _db.TarimIslemler.Update(islem);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var islem = await _db.TarimIslemler.FindAsync(id);
            if (islem is not null)
            {
                _db.TarimIslemler.Remove(islem);
                await _db.SaveChangesAsync();
            }
        }

        public async Task TamamlaAsync(int id, DateTime gerceklesme, decimal? miktar)
        {
            var islem = await _db.TarimIslemler.FindAsync(id);
            if (islem is not null)
            {
                islem.GerceklesenTarih = gerceklesme;
                islem.Miktar = miktar;
                islem.Tamamlandi = true;
                await _db.SaveChangesAsync();
            }
        }

        public async Task<bool> ToplamaMümkünMüAsync(int ekinId)
        {
            var ekin = await _db.Ekinler
                .Include(e => e.EkinTuruNavigation)
                .FirstOrDefaultAsync(e => e.EkinId == ekinId);

            if (ekin?.EkinTuruNavigation?.ToplamaTipi == ToplamaTipi.TekSefer)
            {
                var toplamaMevcutMu = await _db.TarimIslemler
                    .AnyAsync(t => t.EkinId == ekinId && t.IslemTuru == IslemTuru.Toplama);
                return !toplamaMevcutMu;
            }
            return true;
        }
    }
}
