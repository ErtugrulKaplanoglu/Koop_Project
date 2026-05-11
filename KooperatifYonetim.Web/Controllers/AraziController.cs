using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooperatifYonetim.Web.Controllers
{
    [Authorize(Roles = "Yonetici,Uretici,ZiraatMuhendisi,Toptanci")]
    public class AraziController : Controller
    {
        private readonly IAraziService _araziService;
        private readonly IWeatherService _weatherService;
        private readonly UserManager<AppUser> _userManager;

        public AraziController(IAraziService araziService, IWeatherService weatherService, UserManager<AppUser> userManager)
        {
            _araziService = araziService;
            _weatherService = weatherService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var isAdmin = User.IsInRole("Yonetici");
            var araziler = await _araziService.GetListeAsync(userId, isAdmin);
            return View(araziler);
        }

        public async Task<IActionResult> Details(int id)
        {
            var arazi = await _araziService.GetDetayAsync(id);
            if (arazi is null) return NotFound();
            if (!CanView(arazi)) return Forbid();

            ViewBag.HavaDurumu = await _weatherService.GetWeatherAsync(arazi.Enlem, arazi.Boylam);
            return View(arazi);
        }

        [Authorize(Roles = "Yonetici")]
        public async Task<IActionResult> Create()
        {
            var model = new AraziFormViewModel
            {
                UreticiListesi = await GetUreticiListesiAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Yonetici")]
        public async Task<IActionResult> Create(AraziFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.UreticiListesi = await GetUreticiListesiAsync();
                return View(model);
            }

            var arazi = new Arazi
            {
                Ad = model.Ad,
                Enlem = model.Enlem,
                Boylam = model.Boylam,
                YuzOlcumu = model.YuzOlcumu,
                AktifMi = model.AktifMi,
                UreticiId = model.UreticiId
            };

            var id = await _araziService.CreateAsync(arazi);
            TempData["Success"] = $"'{arazi.Ad}' arazisi başarıyla eklendi.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Roles = "Yonetici,Uretici")]
        public async Task<IActionResult> Edit(int id)
        {
            var arazi = await _araziService.GetByIdAsync(id);
            if (arazi is null) return NotFound();
            if (!CanEdit(arazi)) return Forbid();

            var model = new AraziFormViewModel
            {
                AraziId = arazi.AraziId,
                Ad = arazi.Ad,
                Enlem = arazi.Enlem,
                Boylam = arazi.Boylam,
                YuzOlcumu = arazi.YuzOlcumu,
                AktifMi = arazi.AktifMi
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Yonetici,Uretici")]
        public async Task<IActionResult> Edit(AraziFormViewModel model)
        {
            ModelState.Remove("UreticiId");
            if (!ModelState.IsValid) return View(model);

            var arazi = await _araziService.GetByIdAsync(model.AraziId);
            if (arazi is null) return NotFound();
            if (!CanEdit(arazi)) return Forbid();

            arazi.Ad = model.Ad;
            arazi.Enlem = model.Enlem;
            arazi.Boylam = model.Boylam;
            arazi.YuzOlcumu = model.YuzOlcumu;
            arazi.AktifMi = model.AktifMi;

            await _araziService.UpdateAsync(arazi);
            TempData["Success"] = $"'{arazi.Ad}' arazisi güncellendi.";
            return RedirectToAction(nameof(Details), new { id = arazi.AraziId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Yonetici,Uretici")]
        public async Task<IActionResult> Delete(int id)
        {
            var arazi = await _araziService.GetByIdAsync(id);
            if (arazi is null) return NotFound();
            if (!CanEdit(arazi)) return Forbid();

            var ad = arazi.Ad;
            await _araziService.DeleteAsync(id);
            TempData["Success"] = $"'{ad}' arazisi silindi.";
            return RedirectToAction(nameof(Index));
        }

        private bool CanView(Arazi arazi)
        {
            var userId = _userManager.GetUserId(User);
            return User.IsInRole("Yonetici") || User.IsInRole("ZiraatMuhendisi") ||
                   User.IsInRole("Toptanci") || arazi.UreticiId == userId;
        }

        private bool CanEdit(Arazi arazi)
        {
            var userId = _userManager.GetUserId(User);
            return User.IsInRole("Yonetici") || arazi.UreticiId == userId;
        }

        private async Task<IEnumerable<SelectListItem>> GetUreticiListesiAsync()
        {
            var ureticiler = await _userManager.GetUsersInRoleAsync("Uretici");
            return ureticiler
                .OrderBy(u => u.Ad)
                .Select(u => new SelectListItem($"{u.Ad} {u.Soyad} ({u.UserName})", u.Id));
        }
    }
}
