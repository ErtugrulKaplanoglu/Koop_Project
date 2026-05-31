using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Data;
using Microsoft.EntityFrameworkCore;

namespace KooperatifYonetim.Business.Services
{
    public class EkinTuruService : IEkinTuruService
    {
        private readonly AppDbContext _db;

        public EkinTuruService(AppDbContext db) => _db = db;

        public async Task<List<EkinTuru>> GetTumListeAsync()
            => await _db.EkinTurleri.OrderBy(e => e.Ad).ToListAsync();

        public async Task<List<EkinTuru>> GetAktifListeAsync()
            => await _db.EkinTurleri.Where(e => e.AktifMi).OrderBy(e => e.Ad).ToListAsync();

        public async Task<EkinTuru?> GetByIdAsync(int id)
            => await _db.EkinTurleri.FindAsync(id);

        public async Task<int> CreateAsync(EkinTuru ekinTuru)
        {
            _db.EkinTurleri.Add(ekinTuru);
            await _db.SaveChangesAsync();
            return ekinTuru.EkinTuruId;
        }

        public async Task UpdateAsync(EkinTuru ekinTuru)
        {
            _db.EkinTurleri.Update(ekinTuru);
            await _db.SaveChangesAsync();
        }

        public async Task ToggleAktifAsync(int id)
        {
            var ekinTuru = await _db.EkinTurleri.FindAsync(id);
            if (ekinTuru is not null)
            {
                ekinTuru.AktifMi = !ekinTuru.AktifMi;
                await _db.SaveChangesAsync();
            }
        }
    }
}
