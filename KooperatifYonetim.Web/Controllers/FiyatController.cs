using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooperatifYonetim.Web.Controllers
{
    [Authorize(Roles = "Yonetici")]
    public class FiyatController : Controller
    {
        private readonly IUrunFiyatService _fiyatService;
        private readonly IEkinTuruService _ekinTuruService;

        public FiyatController(IUrunFiyatService fiyatService, IEkinTuruService ekinTuruService)
        {
            _fiyatService = fiyatService;
            _ekinTuruService = ekinTuruService;
        }

        public async Task<IActionResult> Index()
            => View(await _fiyatService.GetListeAsync());

        public async Task<IActionResult> Create()
        {
            return View(new UrunFiyatFormViewModel
            {
                EkinTuruListesi = await GetEkinTuruListesiAsync()
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UrunFiyatFormViewModel model)
        {
            if (!model.SutMu && model.EkinTuruId == null)
                ModelState.AddModelError("EkinTuruId", "Ürün ekin türü veya süt seçmelisiniz.");

            if (!ModelState.IsValid)
            {
                model.EkinTuruListesi = await GetEkinTuruListesiAsync();
                return View(model);
            }

            await _fiyatService.CreateAsync(new UrunFiyat
            {
                EkinTuruId = model.SutMu ? null : model.EkinTuruId,
                SutMu = model.SutMu,
                BirimFiyat = model.BirimFiyat,
                BaslangicTarihi = model.BaslangicTarihi
            });
            TempData["Success"] = "Fiyat kaydedildi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _fiyatService.DeleteAsync(id);
            TempData["Success"] = "Fiyat silindi.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetEkinTuruListesiAsync()
        {
            var turler = await _ekinTuruService.GetAktifListeAsync();
            return turler.Select(t => new SelectListItem(t.Ad, t.EkinTuruId.ToString()));
        }
    }
}
