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
    public class VeterinerBakimController : Controller
    {
        private readonly IVeterinerBakimService _bakimService;
        private readonly IAhirService _ahirService;
        private readonly UserManager<AppUser> _userManager;

        public VeterinerBakimController(IVeterinerBakimService bakimService, IAhirService ahirService, UserManager<AppUser> userManager)
        {
            _bakimService = bakimService;
            _ahirService = ahirService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var rolAdi = User.IsInRole("Yonetici") ? "Yonetici"
                       : User.IsInRole("Veteriner") ? "Veteriner"
                       : "Uretici";
            var bakimlar = await _bakimService.GetListeAsync(userId, rolAdi);
            return View(bakimlar);
        }

        [Authorize(Roles = "Yonetici,Uretici")]
        public async Task<IActionResult> Create()
        {
            return View(new VeterinerBakimFormViewModel
            {
                AhirListesi = await GetAhirListesiAsync(),
                VeterinerListesi = await GetVeterinerListesiAsync()
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Yonetici,Uretici")]
        public async Task<IActionResult> Create(VeterinerBakimFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AhirListesi = await GetAhirListesiAsync();
                model.VeterinerListesi = await GetVeterinerListesiAsync();
                return View(model);
            }
            await _bakimService.CreateAsync(new VeterinerBakim
            {
                AhirId = model.AhirId,
                VeterinerId = model.VeterinerId,
                PlanlananTarih = model.PlanlananTarih,
                BakimTuru = model.BakimTuru,
                Notlar = model.Notlar
            });
            TempData["Success"] = "Veteriner bakımı planlandı.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Yonetici,Veteriner")]
        public async Task<IActionResult> Tamamla(int id)
        {
            var bakim = await _bakimService.GetByIdAsync(id);
            if (bakim is null) return NotFound();
            return View(new VeterinerBakimTamamlaViewModel
            {
                BakimId = bakim.BakimId,
                AhirAdi = bakim.Ahir?.Ad ?? string.Empty,
                BakimTuru = bakim.BakimTuru,
                PlanlananTarih = bakim.PlanlananTarih,
                GerceklesenTarih = DateTime.Today,
                Notlar = bakim.Notlar
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Yonetici,Veteriner")]
        public async Task<IActionResult> Tamamla(VeterinerBakimTamamlaViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await _bakimService.TamamlaAsync(model.BakimId, model.GerceklesenTarih, model.Notlar);
            TempData["Success"] = "Bakım tamamlandı olarak işaretlendi.";
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
