using KooperatifYonetim.Core.Enums;

namespace KooperatifYonetim.Core.Entities
{
    public class YemTuru
    {
        public int YemTuruId { get; set; }
        public string Ad { get; set; } = string.Empty;
        public YemBirim Birim { get; set; }
        public bool AktifMi { get; set; } = true;

        public ICollection<BesiStok> BesiStoklar { get; set; } = new List<BesiStok>();
        public ICollection<BesiHareketi> BesiHareketleri { get; set; } = new List<BesiHareketi>();
        public ICollection<YemTedarikBasvuru> TedarikBasvurulari { get; set; } = new List<YemTedarikBasvuru>();
    }
}
