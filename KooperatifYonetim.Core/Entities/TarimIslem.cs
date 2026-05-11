using System.ComponentModel.DataAnnotations;
using KooperatifYonetim.Core.Enums;

namespace KooperatifYonetim.Core.Entities
{
    public class TarimIslem
    {
        [Key]
        public int IslemId { get; set; }
        public int EkinId { get; set; }
        public IslemTuru IslemTuru { get; set; }
        public DateTime PlanlananTarih { get; set; }
        public DateTime? GerceklesenTarih { get; set; }
        public decimal? Miktar { get; set; }
        public string? Notlar { get; set; }
        public bool Tamamlandi { get; set; }

        public Ekin Ekin { get; set; } = null!;
    }
}
