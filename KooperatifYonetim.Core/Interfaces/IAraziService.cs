using KooperatifYonetim.Core.Entities;

namespace KooperatifYonetim.Core.Interfaces
{
    public interface IAraziService
    {
        Task<List<Arazi>> GetListeAsync(string userId, bool isAdmin);
        Task<Arazi?> GetByIdAsync(int id);
        Task<Arazi?> GetDetayAsync(int id);
        Task<int> CreateAsync(Arazi arazi);
        Task UpdateAsync(Arazi arazi);
        Task DeleteAsync(int id);
    }
}
