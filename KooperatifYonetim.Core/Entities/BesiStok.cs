using System.ComponentModel.DataAnnotations;

namespace KooperatifYonetim.Core.Entities
{
    public class BesiStok
    {
        [Key]
        public int StokId { get; set; }
        public int AhirId { get; set; }
        public int YemTuruId { get; set; }
        public decimal MevcutMiktar { get; set; }
        public decimal EsikMiktar { get; set; }
        public DateTime SonGuncelleme { get; set; } = DateTime.Now;

        public Ahir Ahir { get; set; } = null!;
        public YemTuru YemTuru { get; set; } = null!;
    }
}
