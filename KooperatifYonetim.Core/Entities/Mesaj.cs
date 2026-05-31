using System.ComponentModel.DataAnnotations;

namespace KooperatifYonetim.Core.Entities
{
    public class Mesaj
    {
        [Key]
        public int MesajId { get; set; }
        public string GonderenId { get; set; } = string.Empty;
        public string AliciId { get; set; } = string.Empty;
        public string Konu { get; set; } = string.Empty;
        public string Icerik { get; set; } = string.Empty;
        public DateTime GonderimTarihi { get; set; } = DateTime.Now;
        public bool OkunduMu { get; set; } = false;

        public AppUser Gonderen { get; set; } = null!;
        public AppUser Alici { get; set; } = null!;
    }
}
