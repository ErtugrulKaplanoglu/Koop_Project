namespace KooperatifYonetim.Core.Entities
{
    public class Ahir
    {
        public int AhirId { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Adres { get; set; } = string.Empty;
        public int HayvanSayisi { get; set; }
        public string UreticiId { get; set; } = string.Empty;
        public bool AktifMi { get; set; } = true;

        public AppUser Uretici { get; set; } = null!;
        public ICollection<BesiStok> BesiStoklar { get; set; } = new List<BesiStok>();
        public ICollection<BesiHareketi> BesiHareketleri { get; set; } = new List<BesiHareketi>();
        public ICollection<YemTedarikBasvuru> YemTedarikBasvurular { get; set; } = new List<YemTedarikBasvuru>();
        public ICollection<SutUretimi> SutUretimler { get; set; } = new List<SutUretimi>();
        public ICollection<VeterinerBakim> VeterinerBakimlar { get; set; } = new List<VeterinerBakim>();
        public ICollection<HayvanHastalikBildirimi> HayvanHastalikBildirimler { get; set; } = new List<HayvanHastalikBildirimi>();
    }
}
