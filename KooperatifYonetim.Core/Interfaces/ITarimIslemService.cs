using KooperatifYonetim.Core.Entities;

namespace KooperatifYonetim.Core.Interfaces
{
    public interface ITarimIslemService
    {
        Task<List<TarimIslem>> GetListeAsync(string userId, bool isAdmin);
        Task<List<TarimIslem>> GetByEkinAsync(int ekinId);
        Task<TarimIslem?> GetByIdAsync(int id);
        Task<int> CreateAsync(TarimIslem islem);
        Task UpdateAsync(TarimIslem islem);
        Task DeleteAsync(int id);
        Task TamamlaAsync(int id, DateTime gerceklesme, decimal? miktar);
        Task<bool> ToplamaMümkünMüAsync(int ekinId);
    }
}
