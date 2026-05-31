using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KooperatifYonetim.Core.Entities;

namespace KooperatifYonetim.Web.Controllers
{
    [Authorize(Roles = "Yonetici,Uretici")]
    public class OdemeDonemiController : Controller
    {
        private readonly IOdemeDonemiService _odemeDonemiService;
        private readonly UserManager<AppUser> _userManager;

        public OdemeDonemiController(IOdemeDonemiService odemeDonemiService, UserManager<AppUser> userManager)
        {
            _odemeDonemiService = odemeDonemiService;
            _userManager = userManager;
        }

        [Authorize(Roles = "Yonetici")]
        public async Task<IActionResult> Index()
            => View(await _odemeDonemiService.GetListeAsync());

        [Authorize(Roles = "Yonetici")]
        public IActionResult Create()
            => View(new OdemeDonemiFormViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Yonetici")]
        public async Task<IActionResult> Create(OdemeDonemiFormViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var id = await _odemeDonemiService.CreateAsync(model.Yil, model.Ay);
            TempData["Success"] = $"{model.Yil}/{model.Ay:D2} dönemi oluşturuldu.";
            return RedirectToAction(nameof(Detay), new { id });
        }

        [Authorize(Roles = "Yonetici")]
        public async Task<IActionResult> Detay(int id)
        {
            var donem = await _odemeDonemiService.GetDetayAsync(id);
            if (donem is null) return NotFound();
            return View(donem);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Yonetici")]
        public async Task<IActionResult> Hesapla(int id)
        {
            try
            {
                await _odemeDonemiService.HesaplaAsync(id);
                TempData["Success"] = "Ödeme hesaplaması tamamlandı.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Detay), new { id });
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Yonetici")]
        public async Task<IActionResult> Onayla(int id)
        {
            try
            {
                await _odemeDonemiService.OnaylaAsync(id);
                TempData["Success"] = "Dönem kesinleştirildi.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Detay), new { id });
        }

        public async Task<IActionResult> UreticiGecmis()
        {
            var userId = _userManager.GetUserId(User)!;
            var odemeler = await _odemeDonemiService.GetUreticiGecmisAsync(userId);
            return View(odemeler);
        }
    }
}
