using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Data;
using Microsoft.EntityFrameworkCore;

namespace KooperatifYonetim.Business.Services
{
    public class YemTuruService : IYemTuruService
    {
        private readonly AppDbContext _db;

        public YemTuruService(AppDbContext db) => _db = db;

        public async Task<List<YemTuru>> GetTumListeAsync()
            => await _db.YemTurleri.OrderBy(y => y.Ad).ToListAsync();

        public async Task<List<YemTuru>> GetAktifListeAsync()
            => await _db.YemTurleri.Where(y => y.AktifMi).OrderBy(y => y.Ad).ToListAsync();

        public async Task<YemTuru?> GetByIdAsync(int id)
            => await _db.YemTurleri.FindAsync(id);

        public async Task<int> CreateAsync(YemTuru yemTuru)
        {
            _db.YemTurleri.Add(yemTuru);
            await _db.SaveChangesAsync();
            return yemTuru.YemTuruId;
        }

        public async Task UpdateAsync(YemTuru yemTuru)
        {
            _db.YemTurleri.Update(yemTuru);
            await _db.SaveChangesAsync();
        }

        public async Task ToggleAktifAsync(int id)
        {
            var yemTuru = await _db.YemTurleri.FindAsync(id);
            if (yemTuru is not null)
            {
                yemTuru.AktifMi = !yemTuru.AktifMi;
                await _db.SaveChangesAsync();
            }
        }
    }
}
