using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Data;
using Microsoft.EntityFrameworkCore;

namespace KooperatifYonetim.Business.Services
{
    public class SutUretimiService : ISutUretimiService
    {
        private readonly AppDbContext _db;
        public SutUretimiService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<SutUretimi>> GetListeAsync(string userId, string rolAdi)
        {
            var query = _db.SutUretimleri
                .Include(s => s.Ahir).ThenInclude(a => a.Uretici)
                .Include(s => s.Mandira)
                .AsQueryable();
            query = rolAdi switch
            {
                "Yonetici" => query,
                "Mandira"  => query.Where(s => s.MandiraId == userId),
                _          => query.Where(s => s.Ahir.UreticiId == userId)
            };
            return await query.OrderByDescending(s => s.Tarih).Take(200).ToListAsync();
        }

        public async Task CreateAsync(SutUretimi sut)
        {
            _db.SutUretimleri.Add(sut);
            await _db.SaveChangesAsync();
        }
    }
}
