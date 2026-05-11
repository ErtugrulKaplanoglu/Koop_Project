using KooperatifYonetim.Core.Entities;

namespace KooperatifYonetim.Core.Interfaces
{
    public interface ISutUretimiService
    {
        Task<IEnumerable<SutUretimi>> GetListeAsync(string userId, string rolAdi);
        Task CreateAsync(SutUretimi sut);
    }
}
