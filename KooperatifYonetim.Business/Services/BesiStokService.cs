using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Enums;
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
                .Include(b => b.YemTuru)
                .AsQueryable();
            if (!isAdmin)
                query = query.Where(b => b.Ahir.UreticiId == userId);
            return await query.OrderBy(b => b.Ahir.Ad).ThenBy(b => b.YemTuruId).ToListAsync();
        }

        public async Task<BesiStok?> GetByIdAsync(int id) =>
            await _db.BesiStoklar
                .Include(b => b.Ahir).ThenInclude(a => a.Uretici)
                .Include(b => b.YemTuru)
                .FirstOrDefaultAsync(b => b.StokId == id);

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

        public async Task StokEkleAsync(int stokId, decimal miktar, string? notlar)
        {
            var stok = await _db.BesiStoklar
                .Include(b => b.Ahir)
                .Include(b => b.YemTuru)
                .FirstOrDefaultAsync(b => b.StokId == stokId);
            if (stok is null) return;

            _db.BesiHareketleri.Add(new BesiHareketi
            {
                AhirId = stok.AhirId,
                YemTuruId = stok.YemTuruId,
                HareketTipi = HareketTipi.StokEkleme,
                Miktar = miktar,
                Tarih = DateTime.Now,
                Notlar = notlar
            });

            stok.MevcutMiktar += miktar;
            stok.SonGuncelleme = DateTime.Now;

            await _db.SaveChangesAsync();
            await KritikBildirimGonderAsync(stok);
        }

        public async Task TuketimGirAsync(int stokId, decimal miktar, string? notlar)
        {
            var stok = await _db.BesiStoklar
                .Include(b => b.Ahir)
                .Include(b => b.YemTuru)
                .FirstOrDefaultAsync(b => b.StokId == stokId);
            if (stok is null) return;

            stok.MevcutMiktar = Math.Max(0, stok.MevcutMiktar - miktar);
            stok.SonGuncelleme = DateTime.Now;

            _db.BesiHareketleri.Add(new BesiHareketi
            {
                AhirId = stok.AhirId,
                YemTuruId = stok.YemTuruId,
                HareketTipi = HareketTipi.Tuketim,
                Miktar = miktar,
                Tarih = DateTime.Now,
                Notlar = notlar
            });

            await _db.SaveChangesAsync();
            await KritikBildirimGonderAsync(stok);
        }

        public async Task EsikGuncelleAsync(int stokId, decimal esikMiktar)
        {
            var stok = await _db.BesiStoklar.FindAsync(stokId);
            if (stok is null) return;
            stok.EsikMiktar = esikMiktar;
            stok.SonGuncelleme = DateTime.Now;
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<BesiHareketi>> GetHareketlerAsync(int stokId)
        {
            var stok = await _db.BesiStoklar.FindAsync(stokId);
            if (stok is null) return [];
            return await _db.BesiHareketleri
                .Include(h => h.YemTuru)
                .Where(h => h.AhirId == stok.AhirId && h.YemTuruId == stok.YemTuruId)
                .OrderByDescending(h => h.Tarih)
                .Take(50)
                .ToListAsync();
        }

        private async Task KritikBildirimGonderAsync(BesiStok stok)
        {
            if (stok.MevcutMiktar > stok.EsikMiktar) return;

            var sinir = DateTime.Now.AddHours(-24);
            var varMi = await _db.Bildirimler.AnyAsync(b =>
                b.AliciId == stok.Ahir.UreticiId &&
                b.BildirimTipi == BildirimTipi.BesiStok &&
                b.IlgiliKayitId == stok.StokId &&
                !b.Okundu &&
                b.OlusturmaTarihi >= sinir);

            if (varMi) return;

            var birim = stok.YemTuru?.Birim == Core.Enums.YemBirim.Balya ? "balya" : "çuval";
            _db.Bildirimler.Add(new Bildirim
            {
                AliciId = stok.Ahir.UreticiId,
                Baslik = "Besi Stoku Kritik Seviyede",
                Mesaj = $"{stok.Ahir.Ad} ahırındaki {stok.YemTuru?.Ad} stoğu kritik seviyeye düştü. " +
                        $"Mevcut: {stok.MevcutMiktar:N1} {birim}, Eşik: {stok.EsikMiktar:N1} {birim}.",
                BildirimTipi = BildirimTipi.BesiStok,
                IlgiliKayitId = stok.StokId
            });
            await _db.SaveChangesAsync();
        }
    }
}
