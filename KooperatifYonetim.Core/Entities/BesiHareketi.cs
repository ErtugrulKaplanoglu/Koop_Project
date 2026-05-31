using System.ComponentModel.DataAnnotations;
using KooperatifYonetim.Core.Enums;

namespace KooperatifYonetim.Core.Entities
{
    public class BesiHareketi
    {
        [Key]
        public int HareketId { get; set; }
        public int AhirId { get; set; }
        public int YemTuruId { get; set; }
        public HareketTipi HareketTipi { get; set; }
        public decimal Miktar { get; set; }
        public DateTime Tarih { get; set; }
        public string? Notlar { get; set; }

        public Ahir Ahir { get; set; } = null!;
        public YemTuru YemTuru { get; set; } = null!;
    }
}
