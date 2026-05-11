using KooperatifYonetim.Core.Entities;

namespace KooperatifYonetim.Core.Interfaces
{
    public interface IVeterinerBakimService
    {
        Task<IEnumerable<VeterinerBakim>> GetListeAsync(string userId, string rolAdi);
        Task<VeterinerBakim?> GetByIdAsync(int id);
        Task CreateAsync(VeterinerBakim bakim);
        Task TamamlaAsync(int id, DateTime gerceklesenTarih, string? notlar);
    }
}
