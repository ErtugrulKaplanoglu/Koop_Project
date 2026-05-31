using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooperatifYonetim.Web.Controllers
{
    [Authorize(Roles = "Yonetici,Uretici")]
    public class TarimIslemController : Controller
    {
        private readonly ITarimIslemService _islemService;
        private readonly IEkinService _ekinService;
        private readonly UserManager<AppUser> _userManager;

        public TarimIslemController(ITarimIslemService islemService, IEkinService ekinService, UserManager<AppUser> userManager)
        {
            _islemService = islemService;
            _ekinService = ekinService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var islemler = await _islemService.GetListeAsync(userId, User.IsInRole("Yonetici"));
            return View(islemler);
        }

        public async Task<IActionResult> Create(int? ekinId)
        {
            var model = new TarimIslemFormViewModel
            {
                EkinId = ekinId ?? 0,
                EkinListesi = await GetEkinListesiAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TarimIslemFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.EkinListesi = await GetEkinListesiAsync();
                return View(model);
            }

            var islem = new TarimIslem
            {
                EkinId = model.EkinId,
                IslemTuru = model.IslemTuru,
                PlanlananTarih = model.PlanlananTarih,
                GerceklesenTarih = model.GerceklesenTarih,
                Miktar = model.Miktar,
                Notlar = model.Notlar,
                Tamamlandi = model.Tamamlandi
            };

            await _islemService.CreateAsync(islem);
            TempData["Success"] = "Tarım işlemi eklendi.";
            return RedirectToAction("Details", "Ekin", new { id = model.EkinId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var islem = await _islemService.GetByIdAsync(id);
            if (islem is null) return NotFound();

            var model = new TarimIslemFormViewModel
            {
                IslemId = islem.IslemId,
                EkinId = islem.EkinId,
                IslemTuru = islem.IslemTuru,
                PlanlananTarih = islem.PlanlananTarih,
                GerceklesenTarih = islem.GerceklesenTarih,
                Miktar = islem.Miktar,
                Notlar = islem.Notlar,
                Tamamlandi = islem.Tamamlandi,
                EkinListesi = await GetEkinListesiAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TarimIslemFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.EkinListesi = await GetEkinListesiAsync();
                return View(model);
            }

            var islem = await _islemService.GetByIdAsync(model.IslemId);
            if (islem is null) return NotFound();

            islem.EkinId = model.EkinId;
            islem.IslemTuru = model.IslemTuru;
            islem.PlanlananTarih = model.PlanlananTarih;
            islem.GerceklesenTarih = model.GerceklesenTarih;
            islem.Miktar = model.Miktar;
            islem.Notlar = model.Notlar;
            islem.Tamamlandi = model.Tamamlandi;

            await _islemService.UpdateAsync(islem);
            TempData["Success"] = "Tarım işlemi güncellendi.";
            return RedirectToAction("Details", "Ekin", new { id = islem.EkinId });
        }

        public async Task<IActionResult> Tamamla(int id)
        {
            var islem = await _islemService.GetByIdAsync(id);
            if (islem is null) return NotFound();

            var model = new TamamlaViewModel
            {
                IslemId = id,
                IslemTuruAdi = islem.IslemTuru.ToString(),
                PlanlananTarih = islem.PlanlananTarih,
                GerceklesenTarih = DateTime.Today
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Tamamla(TamamlaViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var islem = await _islemService.GetByIdAsync(model.IslemId);
            if (islem is null) return NotFound();

            await _islemService.TamamlaAsync(model.IslemId, model.GerceklesenTarih, model.Miktar);
            TempData["Success"] = "İşlem tamamlandı olarak işaretlendi.";
            return RedirectToAction("Details", "Ekin", new { id = islem.EkinId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var islem = await _islemService.GetByIdAsync(id);
            if (islem is null) return NotFound();

            var ekinId = islem.EkinId;
            await _islemService.DeleteAsync(id);
            TempData["Success"] = "Tarım işlemi silindi.";
            return RedirectToAction("Details", "Ekin", new { id = ekinId });
        }

        private async Task<IEnumerable<SelectListItem>> GetEkinListesiAsync()
        {
            var userId = _userManager.GetUserId(User)!;
            var ekinler = await _ekinService.GetListeAsync(userId, User.IsInRole("Yonetici"));
            return ekinler.Select(e => new SelectListItem($"{e.EkinTuruNavigation?.Ad ?? "?"} — {e.Arazi?.Ad}", e.EkinId.ToString()));
        }
    }
}
