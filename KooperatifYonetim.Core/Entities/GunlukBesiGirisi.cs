using System.ComponentModel.DataAnnotations;

namespace KooperatifYonetim.Core.Entities
{
    public class GunlukBesiGirisi
    {
        [Key]
        public int GirisId { get; set; }
        public int AhirId { get; set; }
        public int YemTuruId { get; set; }
        public decimal YedirildenMiktar { get; set; }
        public DateTime Tarih { get; set; }

        public Ahir Ahir { get; set; } = null!;
        public YemTuru YemTuru { get; set; } = null!;
    }
}
