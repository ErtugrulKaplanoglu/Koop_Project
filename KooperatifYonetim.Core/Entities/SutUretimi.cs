using System.ComponentModel.DataAnnotations;

namespace KooperatifYonetim.Core.Entities
{
    public class SutUretimi
    {
        [Key]
        public int SutId { get; set; }
        public int AhirId { get; set; }
        public string MandiraId { get; set; } = string.Empty;
        public decimal Miktar { get; set; }
        public DateTime Tarih { get; set; }

        public Ahir Ahir { get; set; } = null!;
        public AppUser Mandira { get; set; } = null!;
    }
}
