using KooperatifYonetim.Core.Entities;

namespace KooperatifYonetim.Core.Interfaces
{
    public interface IUrunTeminService
    {
        Task<List<UrunTemin>> GetListeAsync(string userId, bool isAdmin, bool isToptanci);
        Task<UrunTemin?> GetByIdAsync(int id);
        Task<int> CreateAsync(UrunTemin temin);
        Task UpdateAsync(UrunTemin temin);
        Task DeleteAsync(int id);
    }
}
