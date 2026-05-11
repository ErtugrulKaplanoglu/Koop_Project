using KooperatifYonetim.Core.Entities;

namespace KooperatifYonetim.Core.Interfaces
{
    public interface IEkinService
    {
        Task<List<Ekin>> GetListeAsync(string userId, bool isAdmin);
        Task<List<Ekin>> GetByAraziAsync(int araziId);
        Task<Ekin?> GetByIdAsync(int id);
        Task<Ekin?> GetDetayAsync(int id);
        Task<int> CreateAsync(Ekin ekin);
        Task UpdateAsync(Ekin ekin);
        Task DeleteAsync(int id);
    }
}
