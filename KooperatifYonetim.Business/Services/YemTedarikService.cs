using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Enums;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Data;
using Microsoft.EntityFrameworkCore;

namespace KooperatifYonetim.Business.Services
{
    public class YemTedarikService : IYemTedarikService
    {
        private readonly AppDbContext _db;
        public YemTedarikService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<YemTedarikBasvuru>> GetListeAsync(string userId, bool isAdmin)
        {
            var query = _db.YemTedarikBasvurulari
                .Include(y => y.Ahir)
                .Include(y => y.Uretici)
                .Include(y => y.YemTuru)
                .AsQueryable();
            if (!isAdmin)
                query = query.Where(y => y.UreticiId == userId);
            return await query.OrderByDescending(y => y.BasvuruTarihi).ToListAsync();
        }

        public async Task<YemTedarikBasvuru?> GetByIdAsync(int id) =>
            await _db.YemTedarikBasvurulari
                .Include(y => y.Ahir)
                .Include(y => y.Uretici)
                .Include(y => y.YemTuru)
                .FirstOrDefaultAsync(y => y.BasvuruId == id);

        public async Task CreateAsync(YemTedarikBasvuru basvuru, IEnumerable<string> yoneticiIdleri)
        {
            _db.YemTedarikBasvurulari.Add(basvuru);
            await _db.SaveChangesAsync();

            var ahir = await _db.Ahirlar.FindAsync(basvuru.AhirId);
            var yemTuru = await _db.YemTurleri.FindAsync(basvuru.YemTuruId);
            var birim = yemTuru?.Birim == YemBirim.Balya ? "balya" : "çuval";

            foreach (var yoneticiId in yoneticiIdleri)
            {
                _db.Bildirimler.Add(new Bildirim
                {
                    AliciId = yoneticiId,
                    Baslik = "Yeni Yem Tedarik Başvurusu",
                    Mesaj = $"'{ahir?.Ad}' ahırı için {basvuru.TalepMiktar:N1} {birim} {yemTuru?.Ad} tedarik başvurusu yapıldı.",
                    BildirimTipi = BildirimTipi.BesiStok,
                    IlgiliKayitId = basvuru.BasvuruId
                });
            }
            await _db.SaveChangesAsync();
        }

        public async Task DurumGuncelleAsync(int id, BasvuruDurum yeniDurum, string? yoneticiNotu)
        {
            var basvuru = await _db.YemTedarikBasvurulari.FindAsync(id);
            if (basvuru is null) return;
            basvuru.Durum = yeniDurum;
            if (!string.IsNullOrEmpty(yoneticiNotu))
                basvuru.YoneticiNotu = yoneticiNotu;
            await _db.SaveChangesAsync();
        }
    }
}
