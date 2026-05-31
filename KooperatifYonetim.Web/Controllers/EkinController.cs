using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Enums;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooperatifYonetim.Web.Controllers
{
    [Authorize(Roles = "Yonetici,Uretici")]
    public class EkinController : Controller
    {
        private readonly IEkinService _ekinService;
        private readonly IAraziService _araziService;
        private readonly IEkinTuruService _ekinTuruService;
        private readonly UserManager<AppUser> _userManager;

        public EkinController(IEkinService ekinService, IAraziService araziService,
            IEkinTuruService ekinTuruService, UserManager<AppUser> userManager)
        {
            _ekinService = ekinService;
            _araziService = araziService;
            _ekinTuruService = ekinTuruService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var ekinler = await _ekinService.GetListeAsync(userId, User.IsInRole("Yonetici"));
            return View(ekinler);
        }

        public async Task<IActionResult> Details(int id)
        {
            var ekin = await _ekinService.GetDetayAsync(id);
            if (ekin is null) return NotFound();
            if (!CanEdit(ekin)) return Forbid();
            return View(ekin);
        }

        public async Task<IActionResult> Create(int? araziId)
        {
            var model = new EkinFormViewModel
            {
                AraziId = araziId ?? 0,
                AraziListesi = await GetAraziListesiAsync(),
                EkinTuruListesi = await GetEkinTuruListesiAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EkinFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AraziListesi = await GetAraziListesiAsync();
                model.EkinTuruListesi = await GetEkinTuruListesiAsync();
                return View(model);
            }

            var ekin = new Ekin
            {
                AraziId = model.AraziId,
                EkinTuruId = model.EkinTuruId,
                EkimTarihi = model.EkimTarihi,
                HasatTarihi = model.HasatTarihi,
                Durum = model.Durum
            };

            await _ekinService.CreateAsync(ekin);
            var ekinTuru = await _ekinTuruService.GetByIdAsync(model.EkinTuruId);
            TempData["Success"] = $"'{ekinTuru?.Ad}' ekini başarıyla eklendi.";
            return RedirectToAction(nameof(Details), new { id = ekin.EkinId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var ekin = await _ekinService.GetByIdAsync(id);
            if (ekin is null) return NotFound();
            if (!CanEdit(ekin)) return Forbid();

            var model = new EkinFormViewModel
            {
                EkinId = ekin.EkinId,
                AraziId = ekin.AraziId,
                EkinTuruId = ekin.EkinTuruId,
                EkimTarihi = ekin.EkimTarihi,
                HasatTarihi = ekin.HasatTarihi,
                Durum = ekin.Durum,
                AraziListesi = await GetAraziListesiAsync(),
                EkinTuruListesi = await GetEkinTuruListesiAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EkinFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AraziListesi = await GetAraziListesiAsync();
                model.EkinTuruListesi = await GetEkinTuruListesiAsync();
                return View(model);
            }

            var ekin = await _ekinService.GetByIdAsync(model.EkinId);
            if (ekin is null) return NotFound();
            if (!CanEdit(ekin)) return Forbid();

            ekin.AraziId = model.AraziId;
            ekin.EkinTuruId = model.EkinTuruId;
            ekin.EkimTarihi = model.EkimTarihi;
            ekin.HasatTarihi = model.HasatTarihi;
            ekin.Durum = model.Durum;

            await _ekinService.UpdateAsync(ekin);
            var ekinTuru = await _ekinTuruService.GetByIdAsync(model.EkinTuruId);
            TempData["Success"] = $"'{ekinTuru?.Ad}' ekini güncellendi.";
            return RedirectToAction(nameof(Details), new { id = ekin.EkinId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var ekin = await _ekinService.GetByIdAsync(id);
            if (ekin is null) return NotFound();
            if (!CanEdit(ekin)) return Forbid();

            var araziId = ekin.AraziId;
            var tur = ekin.EkinTuruNavigation?.Ad ?? "Ekin";
            await _ekinService.DeleteAsync(id);
            TempData["Success"] = $"'{tur}' ekini silindi.";
            return RedirectToAction("Details", "Arazi", new { id = araziId });
        }

        private bool CanEdit(Ekin ekin)
        {
            var userId = _userManager.GetUserId(User);
            return User.IsInRole("Yonetici") || ekin.Arazi?.UreticiId == userId;
        }

        private async Task<IEnumerable<SelectListItem>> GetAraziListesiAsync()
        {
            var userId = _userManager.GetUserId(User)!;
            var araziler = await _araziService.GetListeAsync(userId, User.IsInRole("Yonetici"));
            return araziler.Select(a => new SelectListItem($"{a.Ad} ({a.YuzOlcumu} dönüm)", a.AraziId.ToString()));
        }

        private async Task<IEnumerable<SelectListItem>> GetEkinTuruListesiAsync()
        {
            var turler = await _ekinTuruService.GetAktifListeAsync();
            return turler.Select(t => new SelectListItem(
                $"{t.Ad} ({(t.ToplamaTipi == ToplamaTipi.Periyodik ? "Periyodik" : "Tek Hasat")})",
                t.EkinTuruId.ToString()));
        }
    }
}
