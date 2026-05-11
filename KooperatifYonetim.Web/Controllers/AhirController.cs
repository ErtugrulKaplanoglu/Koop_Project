using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooperatifYonetim.Web.Controllers
{
    [Authorize(Roles = "Yonetici,Uretici,Veteriner")]
    public class AhirController : Controller
    {
        private readonly IAhirService _ahirService;
        private readonly UserManager<AppUser> _userManager;

        public AhirController(IAhirService ahirService, UserManager<AppUser> userManager)
        {
            _ahirService = ahirService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var isAdmin = User.IsInRole("Yonetici");
            var ahirlar = await _ahirService.GetListeAsync(userId, isAdmin);
            return View(ahirlar);
        }

        public async Task<IActionResult> Details(int id)
        {
            var ahir = await _ahirService.GetByIdAsync(id);
            if (ahir is null) return NotFound();
            var userId = _userManager.GetUserId(User)!;
            if (!User.IsInRole("Yonetici") && !User.IsInRole("Veteriner") && ahir.UreticiId != userId)
                return Forbid();
            return View(ahir);
        }

        [Authorize(Roles = "Yonetici")]
        public async Task<IActionResult> Create()
        {
            var model = new AhirFormViewModel
            {
                UreticiListesi = await GetUreticiListesiAsync()
            };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Yonetici")]
        public async Task<IActionResult> Create(AhirFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.UreticiListesi = await GetUreticiListesiAsync();
                return View(model);
            }
            await _ahirService.CreateAsync(new Ahir
            {
                Ad = model.Ad,
                Adres = model.Adres,
                HayvanSayisi = model.HayvanSayisi,
                UreticiId = model.UreticiId
            });
            TempData["Success"] = "Ahır başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Yonetici,Uretici")]
        public async Task<IActionResult> Edit(int id)
        {
            var ahir = await _ahirService.GetByIdAsync(id);
            if (ahir is null) return NotFound();
            if (!User.IsInRole("Yonetici") && ahir.UreticiId != _userManager.GetUserId(User))
                return Forbid();
            return View(new AhirFormViewModel
            {
                AhirId = ahir.AhirId,
                Ad = ahir.Ad,
                Adres = ahir.Adres,
                HayvanSayisi = ahir.HayvanSayisi
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Yonetici,Uretici")]
        public async Task<IActionResult> Edit(AhirFormViewModel model)
        {
            ModelState.Remove("UreticiId");
            if (!ModelState.IsValid) return View(model);
            var ahir = await _ahirService.GetByIdAsync(model.AhirId);
            if (ahir is null) return NotFound();
            if (!User.IsInRole("Yonetici") && ahir.UreticiId != _userManager.GetUserId(User))
                return Forbid();
            ahir.Ad = model.Ad;
            ahir.Adres = model.Adres;
            ahir.HayvanSayisi = model.HayvanSayisi;
            await _ahirService.UpdateAsync(ahir);
            TempData["Success"] = "Ahır bilgileri güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Yonetici,Uretici")]
        public async Task<IActionResult> Delete(int id)
        {
            var ahir = await _ahirService.GetByIdAsync(id);
            if (ahir is null) return NotFound();
            if (!User.IsInRole("Yonetici") && ahir.UreticiId != _userManager.GetUserId(User))
                return Forbid();
            await _ahirService.DeleteAsync(id);
            TempData["Success"] = "Ahır pasife alındı.";
            return RedirectToAction(nameof(Index));
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
