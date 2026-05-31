using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Data;
using Microsoft.EntityFrameworkCore;

namespace KooperatifYonetim.Business.Services
{
    public class BildirimService : IBildirimService
    {
        private readonly AppDbContext _db;
        public BildirimService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Bildirim>> GetKullaniciBildirimleriAsync(string userId) =>
            await _db.Bildirimler
                .Where(b => b.AliciId == userId)
                .OrderByDescending(b => b.OlusturmaTarihi)
                .ToListAsync();

        public async Task<int> GetOkunmamisSayiAsync(string userId) =>
            await _db.Bildirimler.CountAsync(b => b.AliciId == userId && !b.Okundu);

        public async Task OkunduIsaretleAsync(int bildirimId, string userId)
        {
            var bildirim = await _db.Bildirimler.FindAsync(bildirimId);
            if (bildirim is null || bildirim.AliciId != userId) return;
            bildirim.Okundu = true;
            await _db.SaveChangesAsync();
        }

        public async Task TumunuOkunduIsaretleAsync(string userId)
        {
            var bildirimler = await _db.Bildirimler
                .Where(b => b.AliciId == userId && !b.Okundu)
                .ToListAsync();
            bildirimler.ForEach(b => b.Okundu = true);
            await _db.SaveChangesAsync();
        }

        public async Task CreateAsync(Bildirim bildirim)
        {
            _db.Bildirimler.Add(bildirim);
            await _db.SaveChangesAsync();
        }
    }
}
