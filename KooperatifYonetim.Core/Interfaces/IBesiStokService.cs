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
    }
}
