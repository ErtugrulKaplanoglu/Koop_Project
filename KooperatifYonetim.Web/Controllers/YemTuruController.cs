using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KooperatifYonetim.Web.Controllers
{
    [Authorize(Roles = "Yonetici")]
    public class YemTuruController : Controller
    {
        private readonly IYemTuruService _yemTuruService;

        public YemTuruController(IYemTuruService yemTuruService)
            => _yemTuruService = yemTuruService;

        public async Task<IActionResult> Index()
            => View(await _yemTuruService.GetTumListeAsync());

        public IActionResult Create()
            => View(new YemTuru { AktifMi = true });

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(YemTuru model)
        {
            if (!ModelState.IsValid) return View(model);
            await _yemTuruService.CreateAsync(model);
            TempData["Success"] = $"'{model.Ad}' yem türü eklendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var yemTuru = await _yemTuruService.GetByIdAsync(id);
            if (yemTuru is null) return NotFound();
            return View(yemTuru);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(YemTuru model)
        {
            if (!ModelState.IsValid) return View(model);
            await _yemTuruService.UpdateAsync(model);
            TempData["Success"] = $"'{model.Ad}' yem türü güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAktif(int id)
        {
            await _yemTuruService.ToggleAktifAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
