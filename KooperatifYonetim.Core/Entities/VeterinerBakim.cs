using System.ComponentModel.DataAnnotations;
using KooperatifYonetim.Core.Enums;

namespace KooperatifYonetim.Core.Entities
{
    public class VeterinerBakim
    {
        [Key]
        public int BakimId { get; set; }
        public int AhirId { get; set; }
        public string VeterinerId { get; set; } = string.Empty;
        public DateTime PlanlananTarih { get; set; }
        public DateTime? GerceklesenTarih { get; set; }
        public BakimTuru BakimTuru { get; set; }
        public string? Notlar { get; set; }
        public bool Tamamlandi { get; set; }

        public Ahir Ahir { get; set; } = null!;
        public AppUser Veteriner { get; set; } = null!;
    }
}
