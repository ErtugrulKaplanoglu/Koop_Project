using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooperatifYonetim.Web.Controllers
{
    [Authorize(Roles = "Yonetici")]
    public class KullaniciController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        private static readonly string[] Roller =
        [
            "Yonetici", "Uretici", "ZiraatMuhendisi", "Veteriner",
            "Mandira", "Toptanci", "Tedarikci"
        ];

        public KullaniciController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var liste = new List<KullaniciListeViewModel>();
            foreach (var user in _userManager.Users.OrderBy(u => u.Ad).ToList())
            {
                var roller = await _userManager.GetRolesAsync(user);
                liste.Add(new KullaniciListeViewModel
                {
                    Id = user.Id,
                    Ad = user.Ad,
                    Soyad = user.Soyad,
                    Email = user.Email ?? string.Empty,
                    Telefon = user.Telefon,
                    Rol = roller.FirstOrDefault() ?? "—"
                });
            }
            return View(liste);
        }

        public IActionResult Create()
        {
            return View(new KullaniciFormViewModel { RolListesi = GetRolListesi() });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KullaniciFormViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Sifre))
                ModelState.AddModelError("Sifre", "Yeni kullanıcı için şifre zorunludur.");

            if (!ModelState.IsValid)
            {
                model.RolListesi = GetRolListesi();
                return View(model);
            }

            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                Ad = model.Ad,
                Soyad = model.Soyad,
                Telefon = model.Telefon,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Sifre!);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError(string.Empty, e.Description);
                model.RolListesi = GetRolListesi();
                return View(model);
            }

            await _userManager.AddToRoleAsync(user, model.Rol);
            TempData["Success"] = $"{model.Ad} {model.Soyad} başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();
            var roller = await _userManager.GetRolesAsync(user);
            var mevcutRol = roller.FirstOrDefault() ?? string.Empty;

            return View(new KullaniciFormViewModel
            {
                Id = user.Id,
                Ad = user.Ad,
                Soyad = user.Soyad,
                Email = user.Email ?? string.Empty,
                Telefon = user.Telefon,
                Rol = mevcutRol,
                MevcutRol = mevcutRol,
                RolListesi = GetRolListesi()
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(KullaniciFormViewModel model)
        {
            model.RolListesi = GetRolListesi();

            if (string.IsNullOrEmpty(model.Id)) return NotFound();

            // Şifre alanı opsiyonel, kaldır validasyondan
            ModelState.Remove("Sifre");

            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user is null) return NotFound();

            user.Ad = model.Ad;
            user.Soyad = model.Soyad;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.Telefon = model.Telefon;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var e in updateResult.Errors)
                    ModelState.AddModelError(string.Empty, e.Description);
                return View(model);
            }

            // Şifre değiştir (opsiyonel)
            if (!string.IsNullOrWhiteSpace(model.Sifre))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var pwResult = await _userManager.ResetPasswordAsync(user, token, model.Sifre);
                if (!pwResult.Succeeded)
                {
                    foreach (var e in pwResult.Errors)
                        ModelState.AddModelError(string.Empty, e.Description);
                    return View(model);
                }
            }

            // Rol güncelle
            if (model.Rol != model.MevcutRol)
            {
                var mevcutRoller = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, mevcutRoller);
                await _userManager.AddToRoleAsync(user, model.Rol);
            }

            TempData["Success"] = "Kullanıcı bilgileri güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        private List<SelectListItem> GetRolListesi() =>
            Roller.Select(r => new SelectListItem(r, r)).ToList();
    }
}
