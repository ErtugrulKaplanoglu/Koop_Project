using KooperatifYonetim.Core.DTOs;

namespace KooperatifYonetim.Web.Models
{
    public class UreticiRaporuViewModel
    {
        public int Yil { get; set; }
        public int Ay { get; set; }
        public UreticiAylikOzetDto Ozet { get; set; } = new();
    }

    public class MandiraRaporuViewModel
    {
        public int Yil { get; set; }
        public int Ay { get; set; }
        public List<GunlukSutDto> GunlukSutler { get; set; } = new();
        public decimal ToplamSut => GunlukSutler.Sum(g => g.Miktar);
    }

    public class YoneticiRaporuViewModel
    {
        public int Yil { get; set; }
        public int Ay { get; set; }
        public List<YoneticiUreticiSatirDto> Satirlar { get; set; } = new();
        public decimal ToplamHasatKg => Satirlar.Sum(s => s.ToplamHasatKg);
        public decimal ToplamSutLitre => Satirlar.Sum(s => s.ToplamSutLitre);
        public decimal ToplamOdeme => Satirlar.Sum(s => s.ToplamOdeme);
    }
}
