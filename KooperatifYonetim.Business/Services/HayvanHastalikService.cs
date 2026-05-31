using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Enums;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Data;
using Microsoft.EntityFrameworkCore;

namespace KooperatifYonetim.Business.Services
{
    public class HayvanHastalikService : IHayvanHastalikService
    {
        private readonly AppDbContext _db;
        public HayvanHastalikService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<HayvanHastalikBildirimi>> GetListeAsync(string userId, string rolAdi)
        {
            var query = _db.HayvanHastalikBildirimler
                .Include(h => h.Ahir)
                .Include(h => h.Uretici)
                .Include(h => h.Veteriner)
                .AsQueryable();
            query = rolAdi switch
            {
                "Yonetici"  => query,
                "Veteriner" => query.Where(h => h.VeterinerId == userId || h.VeterinerId == null),
                _           => query.Where(h => h.UreticiId == userId)
            };
            return await query.OrderByDescending(h => h.BildirimTarihi).ToListAsync();
        }

        public async Task<HayvanHastalikBildirimi?> GetByIdAsync(int id) =>
            await _db.HayvanHastalikBildirimler
                .Include(h => h.Ahir)
                .Include(h => h.Uretici)
                .Include(h => h.Veteriner)
                .FirstOrDefaultAsync(h => h.BildirimId == id);

        public async Task CreateAsync(HayvanHastalikBildirimi bildirim)
        {
            _db.HayvanHastalikBildirimler.Add(bildirim);
            await _db.SaveChangesAsync();

            if (!string.IsNullOrEmpty(bildirim.VeterinerId))
            {
                var ahir = await _db.Ahirlar.FindAsync(bildirim.AhirId);
                _db.Bildirimler.Add(new Core.Entities.Bildirim
                {
                    AliciId = bildirim.VeterinerId,
                    Baslik = "Yeni Hayvan Hastalık Bildirimi",
                    Mesaj = $"'{ahir?.Ad ?? "ahır"}' için hayvan hastalık bildirimi oluşturuldu.",
                    BildirimTipi = Core.Enums.BildirimTipi.HayvanHastalik,
                    IlgiliKayitId = bildirim.BildirimId
                });
                await _db.SaveChangesAsync();
            }
        }

        public async Task DurumGuncelleAsync(int id, BildirimDurum yeniDurum, string veterinerId)
        {
            var bildirim = await _db.HayvanHastalikBildirimler.FindAsync(id);
            if (bildirim is null) return;
            bildirim.Durum = yeniDurum;
            if (string.IsNullOrEmpty(bildirim.VeterinerId))
                bildirim.VeterinerId = veterinerId;
            await _db.SaveChangesAsync();
        }
    }
}
