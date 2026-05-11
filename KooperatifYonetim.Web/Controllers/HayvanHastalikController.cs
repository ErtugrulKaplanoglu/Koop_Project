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
    [Authorize(Roles = "Yonetici,Uretici,Veteriner")]
    public class HayvanHastalikController : Controller
    {
        private readonly IHayvanHastalikService _hastalikService;
        private readonly IAhirService _ahirService;
        private readonly UserManager<AppUser> _userManager;

        public HayvanHastalikController(IHayvanHastalikService hastalikService, IAhirService ahirService, UserManager<AppUser> userManager)
        {
            _hastalikService = hastalikService;
            _ahirService = ahirService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var rolAdi = User.IsInRole("Yonetici") ? "Yonetici"
                       : User.IsInRole("Veteriner") ? "Veteriner"
                       : "Uretici";
            var bildirimler = await _hastalikService.GetListeAsync(userId, rolAdi);
            return View(bildirimler);
        }

        [Authorize(Roles = "Yonetici,Uretici")]
        public async Task<IActionResult> Create(int? ahirId)
        {
            return View(new HayvanHastalikFormViewModel
            {
                AhirId = ahirId ?? 0,
                AhirListesi = await GetAhirListesiAsync(),
                VeterinerListesi = await GetVeterinerListesiAsync()
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Yonetici,Uretici")]
        public async Task<IActionResult> Create(HayvanHastalikFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AhirListesi = await GetAhirListesiAsync();
                model.VeterinerListesi = await GetVeterinerListesiAsync();
                return View(model);
            }
            await _hastalikService.CreateAsync(new HayvanHastalikBildirimi
            {
                AhirId = model.AhirId,
                UreticiId = _userManager.GetUserId(User)!,
                VeterinerId = string.IsNullOrEmpty(model.VeterinerId) ? null : model.VeterinerId,
                Aciklama = model.Aciklama,
                BildirimTarihi = DateTime.Now,
                Durum = BildirimDurum.Beklemede
            });
            TempData["Success"] = "Hayvan hastalık bildirimi oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Yonetici,Veteriner")]
        public async Task<IActionResult> DurumGuncelle(int id)
        {
            var bildirim = await _hastalikService.GetByIdAsync(id);
            if (bildirim is null) return NotFound();
            return View(new HayvanDurumGuncelleViewModel
            {
                BildirimId = bildirim.BildirimId,
                AhirAdi = bildirim.Ahir?.Ad ?? string.Empty,
                Aciklama = bildirim.Aciklama,
                MevcutDurum = bildirim.Durum,
                YeniDurum = bildirim.Durum
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Yonetici,Veteriner")]
        public async Task<IActionResult> DurumGuncelle(HayvanDurumGuncelleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await _hastalikService.DurumGuncelleAsync(model.BildirimId, model.YeniDurum, _userManager.GetUserId(User)!);
            TempData["Success"] = "Bildirim durumu güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetAhirListesiAsync()
        {
            var userId = _userManager.GetUserId(User)!;
            var ahirlar = await _ahirService.GetListeAsync(userId, User.IsInRole("Yonetici"));
            return ahirlar.Select(a => new SelectListItem(a.Ad, a.AhirId.ToString()));
        }

        private async Task<IEnumerable<SelectListItem>> GetVeterinerListesiAsync()
        {
            var veterinerler = await _userManager.GetUsersInRoleAsync("Veteriner");
            return veterinerler.Select(v => new SelectListItem($"{v.Ad} {v.Soyad}", v.Id));
        }
    }
}
