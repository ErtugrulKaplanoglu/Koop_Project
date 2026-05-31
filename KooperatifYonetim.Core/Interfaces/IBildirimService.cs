using KooperatifYonetim.Core.Entities;

namespace KooperatifYonetim.Core.Interfaces
{
    public interface IBildirimService
    {
        Task<IEnumerable<Bildirim>> GetKullaniciBildirimleriAsync(string userId);
        Task<int> GetOkunmamisSayiAsync(string userId);
        Task OkunduIsaretleAsync(int bildirimId, string userId);
        Task TumunuOkunduIsaretleAsync(string userId);
        Task CreateAsync(Bildirim bildirim);
    }
}
