using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using KooperatifYonetim.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace KooperatifYonetim.Web.Controllers
{
    [Authorize]
    public class RaporController : Controller
    {
        private readonly IRaporService _raporService;
        private readonly UserManager<AppUser> _userManager;

        public RaporController(IRaporService raporService, UserManager<AppUser> userManager)
        {
            _raporService = raporService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Uretici"))
                return RedirectToAction(nameof(UreticiRaporu));
            if (User.IsInRole("Mandira"))
                return RedirectToAction(nameof(MandiraRaporu));
            if (User.IsInRole("Yonetici"))
                return RedirectToAction(nameof(YoneticiRaporu));
            return RedirectToAction("Index", "Dashboard");
        }

        [Authorize(Roles = "Uretici")]
        public async Task<IActionResult> UreticiRaporu(int? yil, int? ay)
        {
            var bugun = DateTime.Today;
            var secilenYil = yil ?? bugun.Year;
            var secilenAy = ay ?? bugun.Month;
            var userId = _userManager.GetUserId(User)!;

            var ozet = await _raporService.GetUreticiAylikOzetAsync(userId, secilenYil, secilenAy);

            var model = new UreticiRaporuViewModel
            {
                Yil = secilenYil,
                Ay = secilenAy,
                Ozet = ozet
            };
            return View(model);
        }

        [Authorize(Roles = "Mandira")]
        public async Task<IActionResult> MandiraRaporu(int? yil, int? ay)
        {
            var bugun = DateTime.Today;
            var secilenYil = yil ?? bugun.Year;
            var secilenAy = ay ?? bugun.Month;
            var userId = _userManager.GetUserId(User)!;

            var gunlukler = await _raporService.GetMandiraAylikSutAsync(userId, secilenYil, secilenAy);

            var model = new MandiraRaporuViewModel
            {
                Yil = secilenYil,
                Ay = secilenAy,
                GunlukSutler = gunlukler
            };
            return View(model);
        }

        [Authorize(Roles = "Yonetici")]
        public async Task<IActionResult> YoneticiRaporu(int? yil, int? ay)
        {
            var bugun = DateTime.Today;
            var secilenYil = yil ?? bugun.Year;
            var secilenAy = ay ?? bugun.Month;

            var satirlar = await _raporService.GetYoneticiAylikOzetAsync(secilenYil, secilenAy);

            var model = new YoneticiRaporuViewModel
            {
                Yil = secilenYil,
                Ay = secilenAy,
                Satirlar = satirlar
            };
            return View(model);
        }
    }
}
