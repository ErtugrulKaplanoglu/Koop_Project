using KooperatifYonetim.Core.DTOs;
using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Enums;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KooperatifYonetim.Business.Services
{
    public class RaporService : IRaporService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;

        public RaporService(AppDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<UreticiAylikOzetDto> GetUreticiAylikOzetAsync(string ureticiId, int yil, int ay)
        {
            var araziIds = await _db.Araziler
                .Where(a => a.UreticiId == ureticiId)
                .Select(a => a.AraziId)
                .ToListAsync();

            var ekinIds = await _db.Ekinler
                .Where(e => araziIds.Contains(e.AraziId))
                .Select(e => e.EkinId)
                .ToListAsync();

            var islemler = await _db.TarimIslemler
                .Where(t => ekinIds.Contains(t.EkinId)
                         && t.PlanlananTarih.Year == yil
                         && t.PlanlananTarih.Month == ay)
                .ToListAsync();

            var ahirIds = await _db.Ahirlar
                .Where(a => a.UreticiId == ureticiId)
                .Select(a => a.AhirId)
                .ToListAsync();

            var toplamSut = await _db.SutUretimleri
                .Where(s => ahirIds.Contains(s.AhirId)
                         && s.Tarih.Year == yil
                         && s.Tarih.Month == ay)
                .SumAsync(s => (decimal?)s.Miktar) ?? 0m;

            var acikTarimHastalik = await _db.TarimHastalikBildirimler
                .CountAsync(t => t.UreticiId == ureticiId && t.Durum != BildirimDurum.Cozuldu);

            var acikHayvanHastalik = await _db.HayvanHastalikBildirimler
                .CountAsync(h => h.UreticiId == ureticiId && h.Durum != BildirimDurum.Cozuldu);

            return new UreticiAylikOzetDto
            {
                SulamaSayisi = islemler.Count(i => i.IslemTuru == IslemTuru.Sulama),
                IlaclalaSayisi = islemler.Count(i => i.IslemTuru == IslemTuru.Ilacalama),
                ToplamaSayisi = islemler.Count(i => i.IslemTuru == IslemTuru.Toplama),
                ToplamHasatKg = islemler
                    .Where(i => i.IslemTuru == IslemTuru.Toplama && i.Tamamlandi && i.Miktar.HasValue)
                    .Sum(i => i.Miktar!.Value),
                ToplamSutLitre = toplamSut,
                AcikTarimHastalik = acikTarimHastalik,
                AcikHayvanHastalik = acikHayvanHastalik
            };
        }

        public async Task<List<YoneticiUreticiSatirDto>> GetYoneticiAylikOzetAsync(int yil, int ay)
        {
            var ureticiler = await _userManager.GetUsersInRoleAsync("Uretici");

            var donemi = await _db.OdemeDonemleri
                .FirstOrDefaultAsync(o => o.Yil == yil && o.Ay == ay);

            var sonuc = new List<YoneticiUreticiSatirDto>();

            foreach (var uretici in ureticiler)
            {
                decimal hasatKg = 0, sutLitre = 0, odeme = 0;

                if (donemi != null)
                {
                    var odemeSatir = await _db.UreticiOdemeler
                        .FirstOrDefaultAsync(o => o.OdemeDonemiId == donemi.OdemeDonemiId
                                               && o.UreticiId == uretici.Id);
                    if (odemeSatir != null)
                    {
                        hasatKg = odemeSatir.ToplamEkinKg;
                        sutLitre = odemeSatir.ToplamSutLitre;
                        odeme = odemeSatir.ToplamTutar;
                    }
                }

                sonuc.Add(new YoneticiUreticiSatirDto
                {
                    UreticiId = uretici.Id,
                    UreticiAdi = $"{uretici.Ad} {uretici.Soyad}".Trim(),
                    ToplamHasatKg = hasatKg,
                    ToplamSutLitre = sutLitre,
                    ToplamOdeme = odeme
                });
            }

            return sonuc.OrderBy(s => s.UreticiAdi).ToList();
        }

        public async Task<List<GunlukSutDto>> GetMandiraAylikSutAsync(string mandiraId, int yil, int ay)
        {
            var kayitlar = await _db.SutUretimleri
                .Where(s => s.MandiraId == mandiraId
                         && s.Tarih.Year == yil
                         && s.Tarih.Month == ay)
                .ToListAsync();

            return kayitlar
                .GroupBy(s => s.Tarih.Date)
                .Select(g => new GunlukSutDto
                {
                    Tarih = g.Key,
                    Miktar = g.Sum(s => s.Miktar)
                })
                .OrderBy(g => g.Tarih)
                .ToList();
        }
    }
}
