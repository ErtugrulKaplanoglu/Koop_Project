using KooperatifYonetim.Core.Entities;

namespace KooperatifYonetim.Core.Interfaces
{
    public interface IUrunFiyatService
    {
        Task<List<UrunFiyat>> GetListeAsync();
        Task<UrunFiyat?> GetByIdAsync(int id);
        Task CreateAsync(UrunFiyat fiyat);
        Task DeleteAsync(int id);
    }
}
