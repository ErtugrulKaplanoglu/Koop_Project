using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KooperatifYonetim.Business.Services
{
    public class MesajService : IMesajService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;

        public MesajService(AppDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<List<Mesaj>> GetInboxAsync(string userId)
            => await _db.Mesajlar
                .Include(m => m.Gonderen)
                .Where(m => m.AliciId == userId)
                .OrderByDescending(m => m.GonderimTarihi)
                .ToListAsync();

        public async Task<List<Mesaj>> GetGonderilenlerAsync(string userId)
            => await _db.Mesajlar
                .Include(m => m.Alici)
                .Where(m => m.GonderenId == userId)
                .OrderByDescending(m => m.GonderimTarihi)
                .ToListAsync();

        public async Task<Mesaj?> GetMesajDetayAsync(int mesajId, string userId)
        {
            var mesaj = await _db.Mesajlar
                .Include(m => m.Gonderen)
                .Include(m => m.Alici)
                .FirstOrDefaultAsync(m => m.MesajId == mesajId
                                       && (m.AliciId == userId || m.GonderenId == userId));

            if (mesaj != null && mesaj.AliciId == userId && !mesaj.OkunduMu)
            {
                mesaj.OkunduMu = true;
                await _db.SaveChangesAsync();
            }

            return mesaj;
        }

        public async Task SendAsync(Mesaj mesaj)
        {
            _db.Mesajlar.Add(mesaj);
            await _db.SaveChangesAsync();
        }

        public async Task<int> GetOkunmamisSayiAsync(string userId)
            => await _db.Mesajlar.CountAsync(m => m.AliciId == userId && !m.OkunduMu);

        public async Task<List<AppUser>> GetIzinliAlicilarAsync(string gonderenId, IList<string> gonderenRoller)
        {
            var hedefRoller = new List<string>();

            if (gonderenRoller.Contains("Yonetici"))
                hedefRoller.AddRange(new[] { "Uretici", "ZiraatMuhendisi", "Veteriner", "Toptanci", "Mandira", "Tedarikci" });
            else if (gonderenRoller.Contains("Uretici"))
                hedefRoller.AddRange(new[] { "Yonetici", "ZiraatMuhendisi", "Veteriner" });
            else if (gonderenRoller.Contains("ZiraatMuhendisi"))
                hedefRoller.AddRange(new[] { "Yonetici", "Uretici" });
            else if (gonderenRoller.Contains("Veteriner"))
                hedefRoller.AddRange(new[] { "Yonetici", "Uretici" });
            else
                hedefRoller.Add("Yonetici");

            var kullanicilar = new List<AppUser>();
            foreach (var rol in hedefRoller)
            {
                var listesi = await _userManager.GetUsersInRoleAsync(rol);
                kullanicilar.AddRange(listesi.Where(u => u.Id != gonderenId));
            }

            return kullanicilar
                .GroupBy(u => u.Id)
                .Select(g => g.First())
                .OrderBy(u => $"{u.Ad} {u.Soyad}")
                .ToList();
        }
    }
}
