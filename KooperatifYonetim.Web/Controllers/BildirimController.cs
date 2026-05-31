using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KooperatifYonetim.Web.Controllers
{
    [Authorize(Roles = "Uretici,Yonetici,Veteriner,ZiraatMuhendisi")]
    public class BildirimController : Controller
    {
        private readonly IBildirimService _bildirimService;
        private readonly UserManager<AppUser> _userManager;

        public BildirimController(IBildirimService bildirimService, UserManager<AppUser> userManager)
        {
            _bildirimService = bildirimService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var bildirimler = await _bildirimService.GetKullaniciBildirimleriAsync(userId);
            return View(bildirimler);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> OkunduIsaretle(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            await _bildirimService.OkunduIsaretleAsync(id, userId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> TumunuOku()
        {
            var userId = _userManager.GetUserId(User)!;
            await _bildirimService.TumunuOkunduIsaretleAsync(userId);
            TempData["Success"] = "Tüm bildirimler okundu işaretlendi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
