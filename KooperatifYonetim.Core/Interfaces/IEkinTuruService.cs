using KooperatifYonetim.Core.Entities;

namespace KooperatifYonetim.Core.Interfaces
{
    public interface IEkinTuruService
    {
        Task<List<EkinTuru>> GetTumListeAsync();
        Task<List<EkinTuru>> GetAktifListeAsync();
        Task<EkinTuru?> GetByIdAsync(int id);
        Task<int> CreateAsync(EkinTuru ekinTuru);
        Task UpdateAsync(EkinTuru ekinTuru);
        Task ToggleAktifAsync(int id);
    }
}
