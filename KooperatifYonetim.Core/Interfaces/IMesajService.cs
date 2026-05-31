using KooperatifYonetim.Core.Entities;

namespace KooperatifYonetim.Core.Interfaces
{
    public interface IMesajService
    {
        Task<List<Mesaj>> GetInboxAsync(string userId);
        Task<List<Mesaj>> GetGonderilenlerAsync(string userId);
        Task<Mesaj?> GetMesajDetayAsync(int mesajId, string userId);
        Task SendAsync(Mesaj mesaj);
        Task<int> GetOkunmamisSayiAsync(string userId);
        Task<List<AppUser>> GetIzinliAlicilarAsync(string gonderenId, IList<string> gonderenRoller);
    }
}
