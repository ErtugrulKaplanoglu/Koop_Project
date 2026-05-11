using KooperatifYonetim.Core.DTOs;
using KooperatifYonetim.Core.Enums;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Data;
using Microsoft.EntityFrameworkCore;

namespace KooperatifYonetim.Business.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _db;
        public DashboardService(AppDbContext db) => _db = db;

        public async Task<DashboardDataDto> GetYoneticiAsync()
        {
            var acikTarim = await _db.TarimHastalikBildirimler.CountAsync(b => b.Durum != BildirimDurum.Cozuldu);
            var acikHayvan = await _db.HayvanHastalikBildirimler.CountAsync(b => b.Durum != BildirimDurum.Cozuldu);

            return new DashboardDataDto
            {
                ToplamArazi = await _db.Araziler.CountAsync(a => a.AktifMi),
                ToplamAhir = await _db.Ahirlar.CountAsync(a => a.AktifMi),
                ToplamKullanici = await _db.Users.CountAsync(),
                AcikTarimBildirim = acikTarim,
                AcikHayvanBildirim = acikHayvan
            };
        }

        public async Task<DashboardDataDto> GetUreticiAsync(string userId)
        {
            var araziIds = await _db.Araziler
                .Where(a => a.UreticiId == userId && a.AktifMi)
                .Select(a => a.AraziId).ToListAsync();

            var ekinIds = await _db.Ekinler
                .Where(e => araziIds.Contains(e.AraziId))
                .Select(e => e.EkinId).ToListAsync();

            var haftaSonu = DateTime.Today.AddDays(7);
            var buHaftaIslem = await _db.TarimIslemler
                .CountAsync(i => ekinIds.Contains(i.EkinId) && !i.Tamamlandi
                                 && i.PlanlananTarih >= DateTime.Today && i.PlanlananTarih <= haftaSonu);

            var acikTarim = await _db.TarimHastalikBildirimler
                .CountAsync(b => b.UreticiId == userId && b.Durum != BildirimDurum.Cozuldu);

            var ahirIds = await _db.Ahirlar
                .Where(a => a.UreticiId == userId && a.AktifMi)
                .Select(a => a.AhirId).ToListAsync();
            var acikHayvan = await _db.HayvanHastalikBildirimler
                .CountAsync(b => ahirIds.Contains(b.AhirId) && b.Durum != BildirimDurum.Cozuldu);

            var yaklasanIslemler = await _db.TarimIslemler
                .Include(i => i.Ekin).ThenInclude(e => e.Arazi)
                .Where(i => ekinIds.Contains(i.EkinId) && !i.Tamamlandi
                            && i.PlanlananTarih >= DateTime.Today && i.PlanlananTarih <= haftaSonu)
                .OrderBy(i => i.PlanlananTarih)
                .Take(5)
                .Select(i => new YaklasanIslemItemDto
                {
                    EkinTuru = i.Ekin.EkinTuru,
                    AraziAdi = i.Ekin.Arazi != null ? i.Ekin.Arazi.Ad : string.Empty,
                    IslemTuru = i.IslemTuru.ToString(),
                    PlanlananTarih = i.PlanlananTarih
                })
                .ToListAsync();

            return new DashboardDataDto
            {
                AraziSayisi = araziIds.Count,
                AhirSayisi = ahirIds.Count,
                BuHaftaIslem = buHaftaIslem,
                AcikBildirim = acikTarim + acikHayvan,
                YaklasanIslemler = yaklasanIslemler
            };
        }

        public async Task<DashboardDataDto> GetZiraatMuhendisiAsync(string userId)
        {
            var bekleyen = await _db.TarimHastalikBildirimler
                .CountAsync(b => (b.MuhendisId == userId || b.MuhendisId == null) && b.Durum == BildirimDurum.Beklemede);
            var inceleniyor = await _db.TarimHastalikBildirimler
                .CountAsync(b => b.MuhendisId == userId && b.Durum == BildirimDurum.Inceleniyor);
            var cozuldu = await _db.TarimHastalikBildirimler
                .CountAsync(b => b.MuhendisId == userId && b.Durum == BildirimDurum.Cozuldu);

            return new DashboardDataDto
            {
                BekleyenBildirim = bekleyen,
                InceleniyorBildirim = inceleniyor,
                CozulduBildirim = cozuldu
            };
        }

        public async Task<DashboardDataDto> GetVeterinerAsync(string userId)
        {
            var haftaSonu = DateTime.Today.AddDays(7);
            var yaklasan = await _db.VeterinerBakimlar
                .CountAsync(v => v.VeterinerId == userId && !v.Tamamlandi
                                 && v.PlanlananTarih <= haftaSonu);
            var gecikme = await _db.VeterinerBakimlar
                .CountAsync(v => v.VeterinerId == userId && !v.Tamamlandi
                                 && v.PlanlananTarih < DateTime.Today);
            var acikHastalik = await _db.HayvanHastalikBildirimler
                .CountAsync(h => (h.VeterinerId == userId || h.VeterinerId == null) && h.Durum != BildirimDurum.Cozuldu);

            return new DashboardDataDto
            {
                YaklasanBakim = yaklasan,
                GecikmiBakim = gecikme,
                AcikHayvanHastalik = acikHastalik
            };
        }

        public async Task<DashboardDataDto> GetMandiraAsync(string userId)
        {
            var haftaBasi = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1);
            var ayBasi = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            var buHaftaSut = await _db.SutUretimleri
                .Where(s => s.MandiraId == userId && s.Tarih >= haftaBasi)
                .SumAsync(s => (decimal?)s.Miktar) ?? 0;

            var buAySut = await _db.SutUretimleri
                .Where(s => s.MandiraId == userId && s.Tarih >= ayBasi)
                .SumAsync(s => (decimal?)s.Miktar) ?? 0;

            var son7Gun = DateTime.Today.AddDays(-6);
            var gunlukVeriler = await _db.SutUretimleri
                .Where(s => s.MandiraId == userId && s.Tarih >= son7Gun)
                .GroupBy(s => s.Tarih.Date)
                .Select(g => new { Tarih = g.Key, Toplam = g.Sum(s => s.Miktar) })
                .OrderBy(g => g.Tarih)
                .ToListAsync();

            var grafik = Enumerable.Range(0, 7)
                .Select(i => DateTime.Today.AddDays(-6 + i))
                .Select(gun => new SutGunlukDto
                {
                    Gun = gun.ToString("dd.MM"),
                    Miktar = gunlukVeriler.FirstOrDefault(g => g.Tarih == gun.Date)?.Toplam ?? 0
                }).ToList();

            return new DashboardDataDto
            {
                BuHaftaSut = buHaftaSut,
                BuAySut = buAySut,
                HaftalikSutGrafik = grafik
            };
        }

        public async Task<DashboardDataDto> GetToptanciAsync(string userId)
        {
            var aktifTemin = await _db.UrunTeminler.CountAsync(u => u.ToptanciId == userId);
            var toplam = await _db.UrunTeminler
                .Where(u => u.ToptanciId == userId)
                .SumAsync(u => (decimal?)u.PlanlananMiktar) ?? 0;

            return new DashboardDataDto
            {
                AktifTemin = aktifTemin,
                ToplamPlanlananMiktar = toplam
            };
        }

        public async Task<DashboardDataDto> GetTedarikciAsync(string userId)
        {
            var toplam = await _db.BesiStoklar.CountAsync();
            var kritik = await _db.BesiStoklar.CountAsync(s => s.MevcutMiktar < s.EsikMiktar);

            return new DashboardDataDto
            {
                ToplamStok = toplam,
                KritikStok = kritik
            };
        }
    }
}
