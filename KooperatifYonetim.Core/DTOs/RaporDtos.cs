namespace KooperatifYonetim.Core.DTOs
{
    public class UreticiAylikOzetDto
    {
        public int SulamaSayisi { get; set; }
        public int IlaclalaSayisi { get; set; }
        public int ToplamaSayisi { get; set; }
        public decimal ToplamHasatKg { get; set; }
        public decimal ToplamSutLitre { get; set; }
        public int AcikTarimHastalik { get; set; }
        public int AcikHayvanHastalik { get; set; }
    }

    public class YoneticiUreticiSatirDto
    {
        public string UreticiId { get; set; } = string.Empty;
        public string UreticiAdi { get; set; } = string.Empty;
        public decimal ToplamHasatKg { get; set; }
        public decimal ToplamSutLitre { get; set; }
        public decimal ToplamOdeme { get; set; }
    }

    public class GunlukSutDto
    {
        public DateTime Tarih { get; set; }
        public decimal Miktar { get; set; }
    }
}
