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
    public class BesiGirisController : Controller
    {
        private readonly IGunlukBesiGirisiService _girisService;
        private readonly IAhirService _ahirService;
        private readonly UserManager<AppUser> _userManager;

        public BesiGirisController(IGunlukBesiGirisiService girisService, IAhirService ahirService, UserManager<AppUser> userManager)
        {
            _girisService = girisService;
            _ahirService = ahirService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var girisler = await _girisService.GetListeAsync(userId, User.IsInRole("Yonetici"));
            return View(girisler);
        }

        public async Task<IActionResult> Create(int? ahirId)
        {
            return View(new GunlukBesiGirisiFormViewModel
            {
                AhirId = ahirId ?? 0,
                AhirListesi = await GetAhirListesiAsync()
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GunlukBesiGirisiFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AhirListesi = await GetAhirListesiAsync();
                return View(model);
            }
            await _girisService.CreateAsync(new GunlukBesiGirisi
            {
                AhirId = model.AhirId,
                BesiTuru = model.BesiTuru,
                YedirildenMiktar = model.YedirildenMiktar,
                Tarih = model.Tarih
            });
            TempData["Success"] = "Günlük besi girişi kaydedildi.";
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
