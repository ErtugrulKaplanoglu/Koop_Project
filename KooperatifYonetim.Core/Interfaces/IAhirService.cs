using KooperatifYonetim.Core.Entities;

namespace KooperatifYonetim.Core.Interfaces
{
    public interface IAhirService
    {
        Task<IEnumerable<Ahir>> GetListeAsync(string userId, bool isAdmin);
        Task<Ahir?> GetByIdAsync(int id);
        Task CreateAsync(Ahir ahir);
        Task UpdateAsync(Ahir ahir);
        Task DeleteAsync(int id);
    }
}
