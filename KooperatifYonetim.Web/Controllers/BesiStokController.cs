using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooperatifYonetim.Web.Controllers
{
    [Authorize(Roles = "Yonetici,Uretici,Tedarikci")]
    public class BesiStokController : Controller
    {
        private readonly IBesiStokService _stokService;
        private readonly IAhirService _ahirService;
        private readonly UserManager<AppUser> _userManager;

        public BesiStokController(IBesiStokService stokService, IAhirService ahirService, UserManager<AppUser> userManager)
        {
            _stokService = stokService;
            _ahirService = ahirService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var isAdmin = User.IsInRole("Yonetici");
            var stoklar = await _stokService.GetListeAsync(userId, isAdmin);
            return View(stoklar);
        }

        [Authorize(Roles = "Yonetici,Uretici")]
        public async Task<IActionResult> Create()
        {
            return View(new BesiStokFormViewModel { AhirListesi = await GetAhirListesiAsync() });
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Yonetici,Uretici")]
        public async Task<IActionResult> Create(BesiStokFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AhirListesi = await GetAhirListesiAsync();
                return View(model);
            }
            await _stokService.CreateAsync(new BesiStok
            {
                AhirId = model.AhirId,
                BesiTuru = model.BesiTuru,
                MevcutMiktar = model.MevcutMiktar,
                EsikMiktar = model.EsikMiktar
            });
            TempData["Success"] = "Besi stoku eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Yonetici,Uretici")]
        public async Task<IActionResult> Edit(int id)
        {
            var stok = await _stokService.GetByIdAsync(id);
            if (stok is null) return NotFound();
            return View(new BesiStokFormViewModel
            {
                StokId = stok.StokId,
                AhirId = stok.AhirId,
                BesiTuru = stok.BesiTuru,
                MevcutMiktar = stok.MevcutMiktar,
                EsikMiktar = stok.EsikMiktar,
                AhirListesi = await GetAhirListesiAsync()
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Yonetici,Uretici")]
        public async Task<IActionResult> Edit(BesiStokFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AhirListesi = await GetAhirListesiAsync();
                return View(model);
            }
            var stok = await _stokService.GetByIdAsync(model.StokId);
            if (stok is null) return NotFound();
            stok.AhirId = model.AhirId;
            stok.BesiTuru = model.BesiTuru;
            stok.MevcutMiktar = model.MevcutMiktar;
            stok.EsikMiktar = model.EsikMiktar;
            await _stokService.UpdateAsync(stok);
            TempData["Success"] = "Besi stoku güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetAhirListesiAsync()
        {
            var userId = _userManager.GetUserId(User)!;
            var ahirlar = await _ahirService.GetListeAsync(userId, User.IsInRole("Yonetici"));
            return ahirlar.Select(a => new SelectListItem(a.Ad, a.AhirId.ToString()));
        }
    }
}
