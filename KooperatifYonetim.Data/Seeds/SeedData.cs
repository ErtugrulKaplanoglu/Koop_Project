using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        public static async Task SeedEkinTurleriAsync(AppDbContext db)
        {
            if (await db.EkinTurleri.AnyAsync()) return;

            var ekinTurleri = new[]
            {
                new EkinTuru { Ad = "Domates",        ToplamaTipi = ToplamaTipi.Periyodik, AktifMi = true },
                new EkinTuru { Ad = "Biber",          ToplamaTipi = ToplamaTipi.Periyodik, AktifMi = true },
                new EkinTuru { Ad = "Salatalık",      ToplamaTipi = ToplamaTipi.Periyodik, AktifMi = true },
                new EkinTuru { Ad = "Mısır",          ToplamaTipi = ToplamaTipi.Periyodik, AktifMi = true },
                new EkinTuru { Ad = "Fasulye",        ToplamaTipi = ToplamaTipi.Periyodik, AktifMi = true },
                new EkinTuru { Ad = "Çilek",          ToplamaTipi = ToplamaTipi.Periyodik, AktifMi = true },
                new EkinTuru { Ad = "Kabak",          ToplamaTipi = ToplamaTipi.Periyodik, AktifMi = true },
                new EkinTuru { Ad = "Soğan",          ToplamaTipi = ToplamaTipi.TekSefer,  AktifMi = true },
                new EkinTuru { Ad = "Patates",        ToplamaTipi = ToplamaTipi.TekSefer,  AktifMi = true },
                new EkinTuru { Ad = "Şeker Pancarı",  ToplamaTipi = ToplamaTipi.TekSefer,  AktifMi = true },
                new EkinTuru { Ad = "Havuç",          ToplamaTipi = ToplamaTipi.TekSefer,  AktifMi = true },
                new EkinTuru { Ad = "Sarımsak",       ToplamaTipi = ToplamaTipi.TekSefer,  AktifMi = true },
                new EkinTuru { Ad = "Buğday",         ToplamaTipi = ToplamaTipi.TekSefer,  AktifMi = true },
                new EkinTuru { Ad = "Arpa",           ToplamaTipi = ToplamaTipi.TekSefer,  AktifMi = true },
                new EkinTuru { Ad = "Ayçiçeği",       ToplamaTipi = ToplamaTipi.TekSefer,  AktifMi = true },
            };
            db.EkinTurleri.AddRange(ekinTurleri);
            await db.SaveChangesAsync();
        }

        public static async Task SeedYemTurleriAsync(AppDbContext db)
        {
            if (await db.YemTurleri.AnyAsync()) return;

            var yemTurleri = new[]
            {
                new YemTuru { Ad = "Ot",           Birim = YemBirim.Balya, AktifMi = true },
                new YemTuru { Ad = "Saman",        Birim = YemBirim.Balya, AktifMi = true },
                new YemTuru { Ad = "Besi Yemi",    Birim = YemBirim.Cuval, AktifMi = true },
                new YemTuru { Ad = "Arpa Kırması", Birim = YemBirim.Cuval, AktifMi = true },
                new YemTuru { Ad = "Kepek",        Birim = YemBirim.Cuval, AktifMi = true },
                new YemTuru { Ad = "Mısır Kırması",Birim = YemBirim.Cuval, AktifMi = true },
            };
            db.YemTurleri.AddRange(yemTurleri);
            await db.SaveChangesAsync();
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
