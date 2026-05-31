using KooperatifYonetim.Core.Entities;

namespace KooperatifYonetim.Core.Interfaces
{
    public interface IBesiStokService
    {
        Task<IEnumerable<BesiStok>> GetListeAsync(string userId, bool isAdmin);
        Task<BesiStok?> GetByIdAsync(int id);
        Task CreateAsync(BesiStok stok);
        Task UpdateAsync(BesiStok stok);
        Task DeleteAsync(int id);
        Task StokEkleAsync(int stokId, decimal miktar, string? notlar);
        Task TuketimGirAsync(int stokId, decimal miktar, string? notlar);
        Task EsikGuncelleAsync(int stokId, decimal esikMiktar);
        Task<IEnumerable<BesiHareketi>> GetHareketlerAsync(int stokId);
    }
}
