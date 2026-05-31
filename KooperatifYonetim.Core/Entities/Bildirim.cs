using System.ComponentModel.DataAnnotations;
using KooperatifYonetim.Core.Enums;

namespace KooperatifYonetim.Core.Entities
{
    public class Bildirim
    {
        [Key]
        public int BildirimId { get; set; }
        public string AliciId { get; set; } = string.Empty;
        public string Baslik { get; set; } = string.Empty;
        public string Mesaj { get; set; } = string.Empty;
        public BildirimTipi BildirimTipi { get; set; }
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        public bool Okundu { get; set; } = false;
        public int? IlgiliKayitId { get; set; }

        public AppUser Alici { get; set; } = null!;
    }
}
