using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooperatifYonetim.Web.Controllers
{
    [Authorize(Roles = "Yonetici,Uretici,Mandira")]
    public class SutUretimiController : Controller
    {
        private readonly ISutUretimiService _sutService;
        private readonly IAhirService _ahirService;
        private readonly UserManager<AppUser> _userManager;

        public SutUretimiController(ISutUretimiService sutService, IAhirService ahirService, UserManager<AppUser> userManager)
        {
            _sutService = sutService;
            _ahirService = ahirService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var rolAdi = User.IsInRole("Yonetici") ? "Yonetici"
                       : User.IsInRole("Mandira") ? "Mandira"
                       : "Uretici";
            var kayitlar = await _sutService.GetListeAsync(userId, rolAdi);
            return View(kayitlar);
        }

        [Authorize(Roles = "Yonetici,Uretici")]
        public async Task<IActionResult> Create()
        {
            return View(new SutUretimiFormViewModel
            {
                AhirListesi = await GetAhirListesiAsync(),
                MandiraListesi = await GetMandiraListesiAsync()
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Yonetici,Uretici")]
        public async Task<IActionResult> Create(SutUretimiFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AhirListesi = await GetAhirListesiAsync();
                model.MandiraListesi = await GetMandiraListesiAsync();
                return View(model);
            }
            await _sutService.CreateAsync(new SutUretimi
            {
                AhirId = model.AhirId,
                MandiraId = model.MandiraId,
                Miktar = model.Miktar,
                Tarih = model.Tarih
            });
            TempData["Success"] = "Süt üretimi kaydedildi.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetAhirListesiAsync()
        {
            var userId = _userManager.GetUserId(User)!;
            var ahirlar = await _ahirService.GetListeAsync(userId, User.IsInRole("Yonetici"));
            return ahirlar.Select(a => new SelectListItem(a.Ad, a.AhirId.ToString()));
        }

        private async Task<IEnumerable<SelectListItem>> GetMandiraListesiAsync()
        {
            var mandiralar = await _userManager.GetUsersInRoleAsync("Mandira");
            return mandiralar.Select(m => new SelectListItem($"{m.Ad} {m.Soyad}", m.Id));
        }
    }
}
