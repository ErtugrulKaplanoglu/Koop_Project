using System.ComponentModel.DataAnnotations;
using KooperatifYonetim.Core.Enums;

namespace KooperatifYonetim.Core.Entities
{
    public class HayvanHastalikBildirimi
    {
        [Key]
        public int BildirimId { get; set; }
        public int AhirId { get; set; }
        public string UreticiId { get; set; } = string.Empty;
        public string? VeterinerId { get; set; }
        public string Aciklama { get; set; } = string.Empty;
        public DateTime BildirimTarihi { get; set; } = DateTime.Now;
        public BildirimDurum Durum { get; set; } = BildirimDurum.Beklemede;

        public Ahir Ahir { get; set; } = null!;
        public AppUser Uretici { get; set; } = null!;
        public AppUser? Veteriner { get; set; }
    }
}
