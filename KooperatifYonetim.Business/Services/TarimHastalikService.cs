using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Enums;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Data;
using Microsoft.EntityFrameworkCore;

namespace KooperatifYonetim.Business.Services
{
    public class TarimHastalikService : ITarimHastalikService
    {
        private readonly AppDbContext _db;

        public TarimHastalikService(AppDbContext db) => _db = db;

        public async Task<List<TarimHastalikBildirimi>> GetListeAsync(string userId, string rolAdi)
        {
            var query = _db.TarimHastalikBildirimler
                .Include(b => b.Ekin).ThenInclude(e => e.Arazi)
                .Include(b => b.Uretici)
                .Include(b => b.Muhendis)
                .AsQueryable();

            if (rolAdi == "Uretici")
                query = query.Where(b => b.UreticiId == userId);
            else if (rolAdi == "ZiraatMuhendisi")
                query = query.Where(b => b.MuhendisId == userId || b.MuhendisId == null);

            return await query.OrderByDescending(b => b.BildirimTarihi).ToListAsync();
        }

        public async Task<TarimHastalikBildirimi?> GetByIdAsync(int id)
            => await _db.TarimHastalikBildirimler
                .Include(b => b.Ekin).ThenInclude(e => e.Arazi)
                .Include(b => b.Uretici)
                .Include(b => b.Muhendis)
                .FirstOrDefaultAsync(b => b.BildirimId == id);

        public async Task<int> CreateAsync(TarimHastalikBildirimi bildirim)
        {
            _db.TarimHastalikBildirimler.Add(bildirim);
            await _db.SaveChangesAsync();
            return bildirim.BildirimId;
        }

        public async Task DurumGuncelleAsync(int id, BildirimDurum yeniDurum, string muhendisId)
        {
            var bildirim = await _db.TarimHastalikBildirimler.FindAsync(id);
            if (bildirim is not null)
            {
                bildirim.Durum = yeniDurum;
                if (string.IsNullOrEmpty(bildirim.MuhendisId))
                    bildirim.MuhendisId = muhendisId;
                await _db.SaveChangesAsync();
            }
        }
    }
}
