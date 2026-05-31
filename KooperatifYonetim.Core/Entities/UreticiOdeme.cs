namespace KooperatifYonetim.Core.Entities
{
    public class UreticiOdeme
    {
        public int UreticiOdemeId { get; set; }
        public int OdemeDonemiId { get; set; }
        public string UreticiId { get; set; } = string.Empty;
        public decimal ToplamEkinKg { get; set; }
        public decimal ToplamSutLitre { get; set; }
        public decimal ToplamTutar { get; set; }
        public string OdemeDetay { get; set; } = "[]";

        public OdemeDonemi OdemeDonemi { get; set; } = null!;
        public AppUser Uretici { get; set; } = null!;
    }
}
