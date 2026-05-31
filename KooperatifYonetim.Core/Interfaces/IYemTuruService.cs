using KooperatifYonetim.Core.Entities;

namespace KooperatifYonetim.Core.Interfaces
{
    public interface IYemTuruService
    {
        Task<List<YemTuru>> GetTumListeAsync();
        Task<List<YemTuru>> GetAktifListeAsync();
        Task<YemTuru?> GetByIdAsync(int id);
        Task<int> CreateAsync(YemTuru yemTuru);
        Task UpdateAsync(YemTuru yemTuru);
        Task ToggleAktifAsync(int id);
    }
}
