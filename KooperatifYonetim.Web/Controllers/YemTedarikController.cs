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
    public class YemTedarikController : Controller
    {
        private readonly IYemTedarikService _tedarikService;
        private readonly IAhirService _ahirService;
        private readonly IYemTuruService _yemTuruService;
        private readonly UserManager<AppUser> _userManager;

        public YemTedarikController(IYemTedarikService tedarikService, IAhirService ahirService,
            IYemTuruService yemTuruService, UserManager<AppUser> userManager)
        {
            _tedarikService = tedarikService;
            _ahirService = ahirService;
            _yemTuruService = yemTuruService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var basvurular = await _tedarikService.GetListeAsync(userId, User.IsInRole("Yonetici"));
            return View(basvurular);
        }

        [Authorize(Roles = "Uretici")]
        public async Task<IActionResult> Create()
        {
            return View(new YemTedarikFormViewModel
            {
                AhirListesi = await GetAhirListesiAsync(),
                YemTuruListesi = await GetYemTuruListesiAsync()
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Uretici")]
        public async Task<IActionResult> Create(YemTedarikFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AhirListesi = await GetAhirListesiAsync();
                model.YemTuruListesi = await GetYemTuruListesiAsync();
                return View(model);
            }

            var userId = _userManager.GetUserId(User)!;
            var yoneticilar = await _userManager.GetUsersInRoleAsync("Yonetici");

            var basvuru = new YemTedarikBasvuru
            {
                AhirId = model.AhirId,
                UreticiId = userId,
                YemTuruId = model.YemTuruId,
                TalepMiktar = model.TalepMiktar,
                Aciklama = model.Aciklama
            };

            await _tedarikService.CreateAsync(basvuru, yoneticilar.Select(y => y.Id));
            TempData["Success"] = "Yem tedarik başvurunuz iletildi.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Yonetici")]
        public async Task<IActionResult> DurumGuncelle(int id)
        {
            var basvuru = await _tedarikService.GetByIdAsync(id);
            if (basvuru is null) return NotFound();
            ViewBag.DurumListesi = Enum.GetValues<BasvuruDurum>()
                .Select(d => new SelectListItem(d.ToString(), ((int)d).ToString(), d == basvuru.Durum));
            return View(basvuru);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Yonetici")]
        public async Task<IActionResult> DurumGuncelle(int id, BasvuruDurum durum, string? yoneticiNotu)
        {
            await _tedarikService.DurumGuncelleAsync(id, durum, yoneticiNotu);
            TempData["Success"] = "Başvuru durumu güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetAhirListesiAsync()
        {
            var userId = _userManager.GetUserId(User)!;
            var ahirlar = await _ahirService.GetListeAsync(userId, false);
            return ahirlar.Select(a => new SelectListItem(a.Ad, a.AhirId.ToString()));
        }

        private async Task<IEnumerable<SelectListItem>> GetYemTuruListesiAsync()
        {
            var turler = await _yemTuruService.GetAktifListeAsync();
            return turler.Select(t => new SelectListItem(
                $"{t.Ad} ({(t.Birim == Core.Enums.YemBirim.Balya ? "balya" : "çuval")})",
                t.YemTuruId.ToString()));
        }
    }
}
