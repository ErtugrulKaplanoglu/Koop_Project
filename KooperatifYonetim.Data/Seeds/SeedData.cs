using KooperatifYonetim.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace KooperatifYonetim.Data.Seeds
{
    public static class SeedData
    {
        public static readonly string[] Roles =
        {
            "Yonetici", "Uretici", "Veteriner", "ZiraatMuhendisi",
            "Toptanci", "Mandira", "Tedarikci"
        };

        public static async Task SeedRolesAsync(RoleManager<AppRole> roleManager)
        {
            foreach (var roleName in Roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new AppRole { Name = roleName });
                }
            }
        }

        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            var users = new[]
            {
                new { Email = "yonetici@test.com", Ad = "Sistem", Soyad = "Yöneticisi", Rol = "Yonetici" },
                new { Email = "uretici@test.com",  Ad = "Ali",     Soyad = "Üretici",    Rol = "Uretici" },
                new { Email = "veteriner@test.com", Ad = "Ayşe",   Soyad = "Veteriner",  Rol = "Veteriner" },
                new { Email = "muhendis@test.com",  Ad = "Mehmet", Soyad = "Mühendis",   Rol = "ZiraatMuhendisi" },
                new { Email = "toptanci@test.com",  Ad = "Fatma",  Soyad = "Toptancı",   Rol = "Toptanci" },
                new { Email = "mandira@test.com",   Ad = "Hasan",  Soyad = "Mandıra",    Rol = "Mandira" },
                new { Email = "tedarikci@test.com", Ad = "Zeynep", Soyad = "Tedarikçi",  Rol = "Tedarikci" },
            };

            foreach (var u in users)
            {
                if (await userManager.FindByEmailAsync(u.Email) is null)
                {
                    var appUser = new AppUser
                    {
                        UserName = u.Email,
                        Email = u.Email,
                        EmailConfirmed = true,
                        Ad = u.Ad,
                        Soyad = u.Soyad
                    };
                    var result = await userManager.CreateAsync(appUser, "Sifre123!");
                    if (result.Succeeded)
                        await userManager.AddToRoleAsync(appUser, u.Rol);
                }
            }
        }
    }
}
