using System.Text.Json;
using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Enums;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Data;
using Microsoft.EntityFrameworkCore;

namespace KooperatifYonetim.Business.Services
{
    public class OdemeDonemiService : IOdemeDonemiService
    {
        private readonly AppDbContext _db;

        public OdemeDonemiService(AppDbContext db) => _db = db;

        public async Task<List<OdemeDonemi>> GetListeAsync()
            => await _db.OdemeDonemleri
                .OrderByDescending(od => od.Yil).ThenByDescending(od => od.Ay)
                .ToListAsync();

        public async Task<OdemeDonemi?> GetByIdAsync(int id)
            => await _db.OdemeDonemleri.FindAsync(id);

        public async Task<OdemeDonemi?> GetDetayAsync(int id)
            => await _db.OdemeDonemleri
                .Include(od => od.UreticiOdemeler).ThenInclude(uo => uo.Uretici)
                .FirstOrDefaultAsync(od => od.OdemeDonemiId == id);

        public async Task<int> CreateAsync(int yil, int ay)
        {
            var donemi = new OdemeDonemi { Yil = yil, Ay = ay };
            _db.OdemeDonemleri.Add(donemi);
            await _db.SaveChangesAsync();
            return donemi.OdemeDonemiId;
        }

        public async Task HesaplaAsync(int odemeDonemiId)
        {
            var donem = await _db.OdemeDonemleri.FindAsync(odemeDonemiId)
                ?? throw new InvalidOperationException("Dönem bulunamadı.");

            if (donem.Durum == OdemeDonemiDurum.Kesinlesti)
                throw new InvalidOperationException("Kesinleşmiş dönem hesaplanamaz.");

            var ayBaslangic = new DateTime(donem.Yil, donem.Ay, 1);
            var aySonu = ayBaslangic.AddMonths(1);

            // Aktif fiyatları yükle
            var fiyatlar = await _db.UrunFiyatlar
                .Where(f => f.BaslangicTarihi <= aySonu && (f.BitisTarihi == null || f.BitisTarihi >= ayBaslangic))
                .ToListAsync();

            // Tamamlanmış Toplama işlemlerini dönem içinde al
            var toplamalar = await _db.TarimIslemler
                .Include(t => t.Ekin).ThenInclude(e => e.EkinTuruNavigation)
                .Include(t => t.Ekin).ThenInclude(e => e.Arazi)
                .Where(t => t.IslemTuru == IslemTuru.Toplama
                    && t.Tamamlandi
                    && t.GerceklesenTarih >= ayBaslangic
                    && t.GerceklesenTarih < aySonu)
                .ToListAsync();

            // Süt üretimlerini dönem içinde al
            var sutler = await _db.SutUretimleri
                .Include(s => s.Ahir)
                .Where(s => s.Tarih >= ayBaslangic && s.Tarih < aySonu)
                .ToListAsync();

            // Üreticileri grupla
            var ureticiIdler = toplamalar.Select(t => t.Ekin.Arazi.UreticiId)
                .Union(sutler.Select(s => s.Ahir.UreticiId))
                .Distinct();

            // Mevcut hesapları temizle
            var mevcutOdemeler = await _db.UreticiOdemeler
                .Where(uo => uo.OdemeDonemiId == odemeDonemiId)
                .ToListAsync();
            _db.UreticiOdemeler.RemoveRange(mevcutOdemeler);

            foreach (var ureticiId in ureticiIdler)
            {
                var detaylar = new List<object>();
                decimal toplamEkinKg = 0, toplamSutLitre = 0, toplamTutar = 0;

                // Ekin toplamalarını hesapla
                var ureticiToplamalar = toplamalar.Where(t => t.Ekin.Arazi.UreticiId == ureticiId);
                foreach (var toplama in ureticiToplamalar)
                {
                    var kg = toplama.Miktar ?? 0;
                    var ekinTuruId = toplama.Ekin.EkinTuruId;
                    var fiyat = BulAktifFiyat(fiyatlar, ekinTuruId, false, toplama.GerceklesenTarih!.Value);
                    var tutar = kg * fiyat;
                    toplamEkinKg += kg;
                    toplamTutar += tutar;
                    detaylar.Add(new
                    {
                        Tur = "Ekin",
                        Ad = toplama.Ekin.EkinTuruNavigation?.Ad ?? "-",
                        Miktar = kg,
                        BirimFiyat = fiyat,
                        Tutar = tutar
                    });
                }

                // Süt üretimlerini hesapla
                var ureticiSutler = sutler.Where(s => s.Ahir.UreticiId == ureticiId);
                foreach (var sut in ureticiSutler)
                {
                    var litre = sut.Miktar;
                    var fiyat = BulAktifFiyat(fiyatlar, null, true, sut.Tarih);
                    var tutar = litre * fiyat;
                    toplamSutLitre += litre;
                    toplamTutar += tutar;
                    detaylar.Add(new
                    {
                        Tur = "Süt",
                        Ad = "Süt",
                        Miktar = litre,
                        BirimFiyat = fiyat,
                        Tutar = tutar
                    });
                }

                _db.UreticiOdemeler.Add(new UreticiOdeme
                {
                    OdemeDonemiId = odemeDonemiId,
                    UreticiId = ureticiId,
                    ToplamEkinKg = toplamEkinKg,
                    ToplamSutLitre = toplamSutLitre,
                    ToplamTutar = toplamTutar,
                    OdemeDetay = JsonSerializer.Serialize(detaylar)
                });
            }

            await _db.SaveChangesAsync();
        }

        public async Task OnaylaAsync(int odemeDonemiId)
        {
            var donem = await _db.OdemeDonemleri.FindAsync(odemeDonemiId)
                ?? throw new InvalidOperationException("Dönem bulunamadı.");
            donem.Durum = OdemeDonemiDurum.Kesinlesti;
            donem.OnayTarihi = DateTime.Now;
            await _db.SaveChangesAsync();
        }

        public async Task<List<UreticiOdeme>> GetUreticiGecmisAsync(string ureticiId)
            => await _db.UreticiOdemeler
                .Include(uo => uo.OdemeDonemi)
                .Where(uo => uo.UreticiId == ureticiId)
                .OrderByDescending(uo => uo.OdemeDonemi.Yil).ThenByDescending(uo => uo.OdemeDonemi.Ay)
                .ToListAsync();

        private static decimal BulAktifFiyat(List<UrunFiyat> fiyatlar, int? ekinTuruId, bool sutMu, DateTime tarih)
        {
            return fiyatlar
                .Where(f => f.SutMu == sutMu
                    && f.EkinTuruId == ekinTuruId
                    && f.BaslangicTarihi <= tarih
                    && (f.BitisTarihi == null || f.BitisTarihi >= tarih))
                .OrderByDescending(f => f.BaslangicTarihi)
                .Select(f => f.BirimFiyat)
                .FirstOrDefault(0m);
        }
    }
}
