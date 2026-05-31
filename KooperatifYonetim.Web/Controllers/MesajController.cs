using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KooperatifYonetim.Web.Controllers
{
    [Authorize]
    public class MesajController : Controller
    {
        private readonly IMesajService _mesajService;
        private readonly UserManager<AppUser> _userManager;

        public MesajController(IMesajService mesajService, UserManager<AppUser> userManager)
        {
            _mesajService = mesajService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Inbox()
        {
            var userId = _userManager.GetUserId(User)!;
            var mesajlar = await _mesajService.GetInboxAsync(userId);
            return View(mesajlar);
        }

        public async Task<IActionResult> Gonderilenler()
        {
            var userId = _userManager.GetUserId(User)!;
            var mesajlar = await _mesajService.GetGonderilenlerAsync(userId);
            return View(mesajlar);
        }

        public async Task<IActionResult> Detay(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            var mesaj = await _mesajService.GetMesajDetayAsync(id, userId);
            if (mesaj == null) return NotFound();
            return View(mesaj);
        }

        public async Task<IActionResult> YeniMesaj()
        {
            var userId = _userManager.GetUserId(User)!;
            var roller = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(userId) ?? new AppUser());
            var alicilar = await _mesajService.GetIzinliAlicilarAsync(userId, roller);

            return View(new MesajFormViewModel { IzinliAlicilar = alicilar });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> YeniMesaj(MesajFormViewModel form)
        {
            var userId = _userManager.GetUserId(User)!;
            var roller = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(userId) ?? new AppUser());

            if (!ModelState.IsValid)
            {
                form.IzinliAlicilar = await _mesajService.GetIzinliAlicilarAsync(userId, roller);
                return View(form);
            }

            await _mesajService.SendAsync(new Mesaj
            {
                GonderenId = userId,
                AliciId = form.AliciId,
                Konu = form.Konu,
                Icerik = form.Icerik
            });

            TempData["Basari"] = "Mesajınız gönderildi.";
            return RedirectToAction(nameof(Gonderilenler));
        }
    }
}
