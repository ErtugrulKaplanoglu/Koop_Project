namespace KooperatifYonetim.Core.Entities
{
    public class UrunTemin
    {
        public int UrunTeminId { get; set; }
        public int AraziId { get; set; }
        public string ToptanciId { get; set; } = string.Empty;
        public string Donem { get; set; } = string.Empty;
        public decimal PlanlananMiktar { get; set; }
        public decimal? AlinanMiktar { get; set; }

        public Arazi Arazi { get; set; } = null!;
        public AppUser Toptanci { get; set; } = null!;
    }
}
