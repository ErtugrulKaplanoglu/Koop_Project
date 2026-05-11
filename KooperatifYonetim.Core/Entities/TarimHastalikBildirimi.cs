using System.ComponentModel.DataAnnotations;
using KooperatifYonetim.Core.Enums;

namespace KooperatifYonetim.Core.Entities
{
    public class TarimHastalikBildirimi
    {
        [Key]
        public int BildirimId { get; set; }
        public int EkinId { get; set; }
        public string UreticiId { get; set; } = string.Empty;
        public string? MuhendisId { get; set; }
        public string Aciklama { get; set; } = string.Empty;
        public DateTime BildirimTarihi { get; set; } = DateTime.Now;
        public BildirimDurum Durum { get; set; } = BildirimDurum.Beklemede;

        public Ekin Ekin { get; set; } = null!;
        public AppUser Uretici { get; set; } = null!;
        public AppUser? Muhendis { get; set; }
    }
}
