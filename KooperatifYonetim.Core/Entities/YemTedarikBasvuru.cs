using System.ComponentModel.DataAnnotations;
using KooperatifYonetim.Core.Enums;

namespace KooperatifYonetim.Core.Entities
{
    public class YemTedarikBasvuru
    {
        [Key]
        public int BasvuruId { get; set; }
        public int AhirId { get; set; }
        public string UreticiId { get; set; } = string.Empty;
        public int YemTuruId { get; set; }
        public decimal TalepMiktar { get; set; }
        public string? Aciklama { get; set; }
        public DateTime BasvuruTarihi { get; set; } = DateTime.Now;
        public BasvuruDurum Durum { get; set; } = BasvuruDurum.Beklemede;
        public string? YoneticiNotu { get; set; }

        public Ahir Ahir { get; set; } = null!;
        public AppUser Uretici { get; set; } = null!;
        public YemTuru YemTuru { get; set; } = null!;
    }
}
