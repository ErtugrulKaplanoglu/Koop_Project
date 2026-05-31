using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Data;
using Microsoft.EntityFrameworkCore;

namespace KooperatifYonetim.Business.Services
{
    public class UrunFiyatService : IUrunFiyatService
    {
        private readonly AppDbContext _db;

        public UrunFiyatService(AppDbContext db) => _db = db;

        public async Task<List<UrunFiyat>> GetListeAsync()
            => await _db.UrunFiyatlar
                .Include(f => f.EkinTuru)
                .OrderByDescending(f => f.BaslangicTarihi)
                .ToListAsync();

        public async Task<UrunFiyat?> GetByIdAsync(int id)
            => await _db.UrunFiyatlar.Include(f => f.EkinTuru).FirstOrDefaultAsync(f => f.UrunFiyatId == id);

        public async Task CreateAsync(UrunFiyat fiyat)
        {
            // Aynı ürün için mevcut açık fiyatı kapat
            var mevcutAcik = await _db.UrunFiyatlar
                .Where(f => f.SutMu == fiyat.SutMu
                    && f.EkinTuruId == fiyat.EkinTuruId
                    && f.BitisTarihi == null)
                .FirstOrDefaultAsync();

            if (mevcutAcik is not null)
                mevcutAcik.BitisTarihi = DateTime.Today;

            _db.UrunFiyatlar.Add(fiyat);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var fiyat = await _db.UrunFiyatlar.FindAsync(id);
            if (fiyat is not null)
            {
                _db.UrunFiyatlar.Remove(fiyat);
                await _db.SaveChangesAsync();
            }
        }
    }
}
