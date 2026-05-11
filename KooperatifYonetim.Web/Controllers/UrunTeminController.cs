using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooperatifYonetim.Web.Controllers
{
    [Authorize(Roles = "Yonetici,Uretici,Toptanci")]
    public class UrunTeminController : Controller
    {
        private readonly IUrunTeminService _teminService;
        private readonly IAraziService _araziService;
        private readonly UserManager<AppUser> _userManager;

        public UrunTeminController(IUrunTeminService teminService, IAraziService araziService, UserManager<AppUser> userManager)
        {
            _teminService = teminService;
            _araziService = araziService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var teminler = await _teminService.GetListeAsync(
                userId,
                User.IsInRole("Yonetici"),
                User.IsInRole("Toptanci"));
            return View(teminler);
        }

        [Authorize(Roles = "Yonetici,Toptanci")]
        public async Task<IActionResult> Create()
        {
            var model = new UrunTeminFormViewModel
            {
                Donem = $"{DateTime.Today.Year}-{GetDonemAdi()}",
                AraziListesi = await GetAraziListesiAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Yonetici,Toptanci")]
        public async Task<IActionResult> Create(UrunTeminFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AraziListesi = await GetAraziListesiAsync();
                return View(model);
            }

            var temin = new UrunTemin
            {
                AraziId = model.AraziId,
                ToptanciId = _userManager.GetUserId(User)!,
                Donem = model.Donem,
                PlanlananMiktar = model.PlanlananMiktar,
                AlinanMiktar = model.AlinanMiktar
            };

            await _teminService.CreateAsync(temin);
            TempData["Success"] = "Ürün temin kaydı oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Yonetici,Toptanci")]
        public async Task<IActionResult> Edit(int id)
        {
            var temin = await _teminService.GetByIdAsync(id);
            if (temin is null) return NotFound();
            if (!CanEdit(temin)) return Forbid();

            var model = new UrunTeminFormViewModel
            {
                UrunTeminId = temin.UrunTeminId,
                AraziId = temin.AraziId,
                Donem = temin.Donem,
                PlanlananMiktar = temin.PlanlananMiktar,
                AlinanMiktar = temin.AlinanMiktar,
                AraziListesi = await GetAraziListesiAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Yonetici,Toptanci")]
        public async Task<IActionResult> Edit(UrunTeminFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AraziListesi = await GetAraziListesiAsync();
                return View(model);
            }

            var temin = await _teminService.GetByIdAsync(model.UrunTeminId);
            if (temin is null) return NotFound();
            if (!CanEdit(temin)) return Forbid();

            temin.AraziId = model.AraziId;
            temin.Donem = model.Donem;
            temin.PlanlananMiktar = model.PlanlananMiktar;
            temin.AlinanMiktar = model.AlinanMiktar;

            await _teminService.UpdateAsync(temin);
            TempData["Success"] = "Ürün temin kaydı güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Yonetici,Toptanci")]
        public async Task<IActionResult> Delete(int id)
        {
            var temin = await _teminService.GetByIdAsync(id);
            if (temin is null) return NotFound();
            if (!CanEdit(temin)) return Forbid();

            await _teminService.DeleteAsync(id);
            TempData["Success"] = "Ürün temin kaydı silindi.";
            return RedirectToAction(nameof(Index));
        }

        private bool CanEdit(UrunTemin temin)
        {
            var userId = _userManager.GetUserId(User);
            return User.IsInRole("Yonetici") || temin.ToptanciId == userId;
        }

        private string GetDonemAdi()
        {
            var ay = DateTime.Today.Month;
            return ay <= 3 ? "Kış" : ay <= 6 ? "İlkbahar" : ay <= 9 ? "Yaz" : "Sonbahar";
        }

        private async Task<IEnumerable<SelectListItem>> GetAraziListesiAsync()
        {
            // Toptancı tüm aktif arazileri görebilir
            var araziler = await _araziService.GetListeAsync(string.Empty, isAdmin: true);
            return araziler.Where(a => a.AktifMi)
                .Select(a => new SelectListItem($"{a.Ad} — {a.Uretici?.Ad} {a.Uretici?.Soyad} ({a.YuzOlcumu} dönüm)", a.AraziId.ToString()));
        }
    }
}
