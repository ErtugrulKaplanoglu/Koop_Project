using System.ComponentModel.DataAnnotations;

namespace KooperatifYonetim.Core.Entities
{
    public class GunlukBesiGirisi
    {
        [Key]
        public int GirisId { get; set; }
        public int AhirId { get; set; }
        public string BesiTuru { get; set; } = string.Empty;
        public decimal YedirildenMiktar { get; set; }
        public DateTime Tarih { get; set; }

        public Ahir Ahir { get; set; } = null!;
    }
}
