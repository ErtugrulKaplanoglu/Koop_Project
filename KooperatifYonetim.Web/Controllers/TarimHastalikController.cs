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
    [Authorize(Roles = "Yonetici,Uretici,ZiraatMuhendisi")]
    public class TarimHastalikController : Controller
    {
        private readonly ITarimHastalikService _hastalikService;
        private readonly IEkinService _ekinService;
        private readonly UserManager<AppUser> _userManager;

        public TarimHastalikController(ITarimHastalikService hastalikService, IEkinService ekinService, UserManager<AppUser> userManager)
        {
            _hastalikService = hastalikService;
            _ekinService = ekinService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var rolAdi = User.IsInRole("Yonetici") ? "Yonetici"
                       : User.IsInRole("ZiraatMuhendisi") ? "ZiraatMuhendisi"
                       : "Uretici";

            var bildirimler = await _hastalikService.GetListeAsync(userId, rolAdi);
            return View(bildirimler);
        }

        [Authorize(Roles = "Yonetici,Uretici")]
        public async Task<IActionResult> Create(int? ekinId)
        {
            var model = new TarimHastalikFormViewModel
            {
                EkinId = ekinId ?? 0,
                EkinListesi = await GetEkinListesiAsync(),
                MuhendisListesi = await GetMuhendisListesiAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Yonetici,Uretici")]
        public async Task<IActionResult> Create(TarimHastalikFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.EkinListesi = await GetEkinListesiAsync();
                model.MuhendisListesi = await GetMuhendisListesiAsync();
                return View(model);
            }

            var bildirim = new TarimHastalikBildirimi
            {
                EkinId = model.EkinId,
                UreticiId = _userManager.GetUserId(User)!,
                MuhendisId = string.IsNullOrEmpty(model.MuhendisId) ? null : model.MuhendisId,
                Aciklama = model.Aciklama,
                BildirimTarihi = DateTime.Now,
                Durum = BildirimDurum.Beklemede
            };

            await _hastalikService.CreateAsync(bildirim);
            TempData["Success"] = "Hastalık bildirimi oluşturuldu. Ziraat mühendisi en kısa sürede inceleyecektir.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Yonetici,ZiraatMuhendisi")]
        public async Task<IActionResult> DurumGuncelle(int id)
        {
            var bildirim = await _hastalikService.GetByIdAsync(id);
            if (bildirim is null) return NotFound();

            var model = new DurumGuncelleViewModel
            {
                BildirimId = id,
                EkinTuru = bildirim.Ekin?.EkinTuruNavigation?.Ad ?? string.Empty,
                AraziAdi = bildirim.Ekin?.Arazi?.Ad ?? string.Empty,
                Aciklama = bildirim.Aciklama,
                MevcutDurum = bildirim.Durum,
                YeniDurum = bildirim.Durum
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Yonetici,ZiraatMuhendisi")]
        public async Task<IActionResult> DurumGuncelle(DurumGuncelleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = _userManager.GetUserId(User)!;
            await _hastalikService.DurumGuncelleAsync(model.BildirimId, model.YeniDurum, userId);
            TempData["Success"] = "Bildirim durumu güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetEkinListesiAsync()
        {
            var userId = _userManager.GetUserId(User)!;
            var ekinler = await _ekinService.GetListeAsync(userId, User.IsInRole("Yonetici"));
            return ekinler.Select(e => new SelectListItem($"{e.EkinTuruNavigation?.Ad ?? "?"} — {e.Arazi?.Ad}", e.EkinId.ToString()));
        }

        private async Task<IEnumerable<SelectListItem>> GetMuhendisListesiAsync()
        {
            var muhendisler = await _userManager.GetUsersInRoleAsync("ZiraatMuhendisi");
            return muhendisler.Select(m => new SelectListItem($"{m.Ad} {m.Soyad}", m.Id));
        }
    }
}
