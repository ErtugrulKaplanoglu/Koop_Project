using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Data;
using Microsoft.EntityFrameworkCore;

namespace KooperatifYonetim.Business.Services
{
    public class VeterinerBakimService : IVeterinerBakimService
    {
        private readonly AppDbContext _db;
        public VeterinerBakimService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<VeterinerBakim>> GetListeAsync(string userId, string rolAdi)
        {
            var query = _db.VeterinerBakimlar
                .Include(v => v.Ahir).ThenInclude(a => a.Uretici)
                .Include(v => v.Veteriner)
                .AsQueryable();
            query = rolAdi switch
            {
                "Yonetici"  => query,
                "Veteriner" => query.Where(v => v.VeterinerId == userId),
                _           => query.Where(v => v.Ahir.UreticiId == userId)
            };
            return await query.OrderBy(v => v.Tamamlandi).ThenBy(v => v.PlanlananTarih).ToListAsync();
        }

        public async Task<VeterinerBakim?> GetByIdAsync(int id) =>
            await _db.VeterinerBakimlar
                .Include(v => v.Ahir)
                .Include(v => v.Veteriner)
                .FirstOrDefaultAsync(v => v.BakimId == id);

        public async Task CreateAsync(VeterinerBakim bakim)
        {
            _db.VeterinerBakimlar.Add(bakim);
            await _db.SaveChangesAsync();
        }

        public async Task TamamlaAsync(int id, DateTime gerceklesenTarih, string? notlar)
        {
            var bakim = await _db.VeterinerBakimlar.FindAsync(id);
            if (bakim is null) return;
            bakim.GerceklesenTarih = gerceklesenTarih;
            bakim.Notlar = notlar;
            bakim.Tamamlandi = true;
            await _db.SaveChangesAsync();
        }
    }
}
