using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Enums;
using KooperatifYonetim.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooperatifYonetim.Web.Controllers
{
    [Authorize(Roles = "Yonetici")]
    public class EkinTuruController : Controller
    {
        private readonly IEkinTuruService _ekinTuruService;

        public EkinTuruController(IEkinTuruService ekinTuruService)
            => _ekinTuruService = ekinTuruService;

        public async Task<IActionResult> Index()
            => View(await _ekinTuruService.GetTumListeAsync());

        public IActionResult Create()
            => View(new EkinTuru { AktifMi = true });

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EkinTuru model)
        {
            if (!ModelState.IsValid) return View(model);
            await _ekinTuruService.CreateAsync(model);
            TempData["Success"] = $"'{model.Ad}' ekin türü eklendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var ekinTuru = await _ekinTuruService.GetByIdAsync(id);
            if (ekinTuru is null) return NotFound();
            return View(ekinTuru);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EkinTuru model)
        {
            if (!ModelState.IsValid) return View(model);
            await _ekinTuruService.UpdateAsync(model);
            TempData["Success"] = $"'{model.Ad}' ekin türü güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAktif(int id)
        {
            await _ekinTuruService.ToggleAktifAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
