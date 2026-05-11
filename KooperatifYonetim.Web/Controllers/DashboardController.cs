using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using KooperatifYonetim.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace KooperatifYonetim.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly UserManager<AppUser> _userManager;

        public DashboardController(IDashboardService dashboardService, UserManager<AppUser> userManager)
        {
            _dashboardService = dashboardService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;

            var data = User.IsInRole("Yonetici")       ? await _dashboardService.GetYoneticiAsync()
                     : User.IsInRole("Uretici")        ? await _dashboardService.GetUreticiAsync(userId)
                     : User.IsInRole("ZiraatMuhendisi")? await _dashboardService.GetZiraatMuhendisiAsync(userId)
                     : User.IsInRole("Veteriner")      ? await _dashboardService.GetVeterinerAsync(userId)
                     : User.IsInRole("Mandira")        ? await _dashboardService.GetMandiraAsync(userId)
                     : User.IsInRole("Toptanci")       ? await _dashboardService.GetToptanciAsync(userId)
                     : User.IsInRole("Tedarikci")      ? await _dashboardService.GetTedarikciAsync(userId)
                     : new KooperatifYonetim.Core.DTOs.DashboardDataDto();

            var vm = new DashboardViewModel
            {
                ToplamArazi = data.ToplamArazi,
                ToplamAhir = data.ToplamAhir,
                ToplamKullanici = data.ToplamKullanici,
                AcikTarimBildirim = data.AcikTarimBildirim,
                AcikHayvanBildirim = data.AcikHayvanBildirim,
                AraziSayisi = data.AraziSayisi,
                AhirSayisi = data.AhirSayisi,
                BuHaftaIslem = data.BuHaftaIslem,
                AcikBildirim = data.AcikBildirim,
                YaklasanIslemler = data.YaklasanIslemler.Select(i => new YaklasanIslemDto
                {
                    EkinTuru = i.EkinTuru,
                    AraziAdi = i.AraziAdi,
                    IslemTuru = i.IslemTuru,
                    PlanlananTarih = i.PlanlananTarih
                }).ToList(),
                BekleyenBildirim = data.BekleyenBildirim,
                InceleniyorBildirim = data.InceleniyorBildirim,
                CozulduBildirim = data.CozulduBildirim,
                YaklasanBakim = data.YaklasanBakim,
                GecikmiBakim = data.GecikmiBakim,
                AcikHayvanHastalik = data.AcikHayvanHastalik,
                BuHaftaSut = data.BuHaftaSut,
                BuAySut = data.BuAySut,
                HaftalikSutGrafik = data.HaftalikSutGrafik.Select(g => new SutGrafikDto
                {
                    Gun = g.Gun,
                    Miktar = g.Miktar
                }).ToList(),
                AktifTemin = data.AktifTemin,
                ToplamPlanlananMiktar = data.ToplamPlanlananMiktar,
                KritikStok = data.KritikStok,
                ToplamStok = data.ToplamStok
            };

            return View(vm);
        }
    }
}
