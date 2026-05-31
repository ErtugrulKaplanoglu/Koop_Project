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
    public class BesiStokController : Controller
    {
        private readonly IBesiStokService _stokService;
        private readonly IAhirService _ahirService;
        private readonly IYemTuruService _yemTuruService;
        private readonly UserManager<AppUser> _userManager;

        public BesiStokController(IBesiStokService stokService, IAhirService ahirService,
            IYemTuruService yemTuruService, UserManager<AppUser> userManager)
        {
            _stokService = stokService;
            _ahirService = ahirService;
            _yemTuruService = yemTuruService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var stoklar = await _stokService.GetListeAsync(userId, User.IsInRole("Yonetici"));
            return View(stoklar);
        }

        public async Task<IActionResult> Create()
        {
            return View(new BesiStokFormViewModel
            {
                AhirListesi = await GetAhirListesiAsync(),
                YemTuruListesi = await GetYemTuruListesiAsync()
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BesiStokFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AhirListesi = await GetAhirListesiAsync();
                model.YemTuruListesi = await GetYemTuruListesiAsync();
                return View(model);
            }
            await _stokService.CreateAsync(new BesiStok
            {
                AhirId = model.AhirId,
                YemTuruId = model.YemTuruId,
                MevcutMiktar = model.MevcutMiktar,
                EsikMiktar = model.EsikMiktar
            });
            TempData["Success"] = "Besi stoku eklendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var stok = await _stokService.GetByIdAsync(id);
            if (stok is null) return NotFound();
            return View(new BesiStokFormViewModel
            {
                StokId = stok.StokId,
                AhirId = stok.AhirId,
                YemTuruId = stok.YemTuruId,
                MevcutMiktar = stok.MevcutMiktar,
                EsikMiktar = stok.EsikMiktar,
                AhirListesi = await GetAhirListesiAsync(),
                YemTuruListesi = await GetYemTuruListesiAsync()
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BesiStokFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AhirListesi = await GetAhirListesiAsync();
                model.YemTuruListesi = await GetYemTuruListesiAsync();
                return View(model);
            }
            var stok = await _stokService.GetByIdAsync(model.StokId);
            if (stok is null) return NotFound();
            stok.AhirId = model.AhirId;
            stok.YemTuruId = model.YemTuruId;
            stok.MevcutMiktar = model.MevcutMiktar;
            stok.EsikMiktar = model.EsikMiktar;
            await _stokService.UpdateAsync(stok);
            TempData["Success"] = "Besi stoku güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> StokEkle(int id)
        {
            var stok = await _stokService.GetByIdAsync(id);
            if (stok is null) return NotFound();
            return View(BuildHareketiVM(stok));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> StokEkle(BesiHareketiFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var s = await _stokService.GetByIdAsync(model.StokId);
                if (s is not null) PopulateHareketiVM(model, s);
                return View(model);
            }
            await _stokService.StokEkleAsync(model.StokId, model.Miktar, model.Notlar);
            TempData["Success"] = $"{model.Miktar:N1} {model.Birim} stok eklendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> TuketimGir(int id)
        {
            var stok = await _stokService.GetByIdAsync(id);
            if (stok is null) return NotFound();
            return View(BuildHareketiVM(stok));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> TuketimGir(BesiHareketiFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var s = await _stokService.GetByIdAsync(model.StokId);
                if (s is not null) PopulateHareketiVM(model, s);
                return View(model);
            }
            await _stokService.TuketimGirAsync(model.StokId, model.Miktar, model.Notlar);
            TempData["Success"] = $"{model.Miktar:N1} {model.Birim} tüketim kaydedildi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EsikGuncelle(int id)
        {
            var stok = await _stokService.GetByIdAsync(id);
            if (stok is null) return NotFound();
            var birim = BirimStr(stok);
            return View(new EsikGuncelleViewModel
            {
                StokId = stok.StokId,
                EsikMiktar = stok.EsikMiktar,
                AhirAd = stok.Ahir?.Ad ?? string.Empty,
                YemTuruAd = stok.YemTuru?.Ad ?? string.Empty,
                Birim = birim,
                MevcutMiktar = stok.MevcutMiktar
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EsikGuncelle(EsikGuncelleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await _stokService.EsikGuncelleAsync(model.StokId, model.EsikMiktar);
            TempData["Success"] = "Eşik miktarı güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        private static string BirimStr(BesiStok stok) =>
            stok.YemTuru?.Birim == Core.Enums.YemBirim.Balya ? "balya" : "çuval";

        private static BesiHareketiFormViewModel BuildHareketiVM(BesiStok stok) => new()
        {
            StokId = stok.StokId,
            AhirAd = stok.Ahir?.Ad ?? string.Empty,
            YemTuruAd = stok.YemTuru?.Ad ?? string.Empty,
            Birim = BirimStr(stok),
            MevcutMiktar = stok.MevcutMiktar,
            EsikMiktar = stok.EsikMiktar
        };

        private static void PopulateHareketiVM(BesiHareketiFormViewModel vm, BesiStok stok)
        {
            vm.AhirAd = stok.Ahir?.Ad ?? string.Empty;
            vm.YemTuruAd = stok.YemTuru?.Ad ?? string.Empty;
            vm.Birim = BirimStr(stok);
            vm.MevcutMiktar = stok.MevcutMiktar;
            vm.EsikMiktar = stok.EsikMiktar;
        }

        private async Task<IEnumerable<SelectListItem>> GetAhirListesiAsync()
        {
            var userId = _userManager.GetUserId(User)!;
            var ahirlar = await _ahirService.GetListeAsync(userId, User.IsInRole("Yonetici"));
            return ahirlar.Select(a => new SelectListItem(a.Ad, a.AhirId.ToString()));
        }

        private async Task<IEnumerable<SelectListItem>> GetYemTuruListesiAsync()
        {
            var turler = await _yemTuruService.GetAktifListeAsync();
            ViewBag.YemBirimMap = System.Text.Json.JsonSerializer.Serialize(
                turler.ToDictionary(t => t.YemTuruId, t => t.Birim == Core.Enums.YemBirim.Balya ? "balya" : "çuval"));
            return turler.Select(t => new SelectListItem(
                $"{t.Ad} ({(t.Birim == Core.Enums.YemBirim.Balya ? "balya" : "çuval")})",
                t.YemTuruId.ToString()));
        }
    }
}
