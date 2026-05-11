using KooperatifYonetim.Core.Enums;

namespace KooperatifYonetim.Core.Entities
{
    public class Ekin
    {
        public int EkinId { get; set; }
        public int AraziId { get; set; }
        public string EkinTuru { get; set; } = string.Empty;
        public DateTime EkimTarihi { get; set; }
        public DateTime? HasatTarihi { get; set; }
        public EkinDurum Durum { get; set; } = EkinDurum.Aktif;

        public Arazi Arazi { get; set; } = null!;
        public ICollection<TarimIslem> TarimIslemler { get; set; } = new List<TarimIslem>();
        public ICollection<TarimHastalikBildirimi> HastalıkBildirimler { get; set; } = new List<TarimHastalikBildirimi>();
    }
}
