using KooperatifYonetim.Core.Enums;

namespace KooperatifYonetim.Web.Models
{
    public class DashboardViewModel
    {
        // Yönetici
        public int ToplamArazi { get; set; }
        public int ToplamAhir { get; set; }
        public int ToplamKullanici { get; set; }
        public int AcikTarimBildirim { get; set; }
        public int AcikHayvanBildirim { get; set; }

        // Üretici
        public int AraziSayisi { get; set; }
        public int AhirSayisi { get; set; }
        public int BuHaftaIslem { get; set; }
        public int AcikBildirim { get; set; }
        public List<YaklasanIslemDto> YaklasanIslemler { get; set; } = [];

        // Ziraat Mühendisi
        public int BekleyenBildirim { get; set; }
        public int InceleniyorBildirim { get; set; }
        public int CozulduBildirim { get; set; }

        // Veteriner
        public int YaklasanBakim { get; set; }
        public int GecikmiBakim { get; set; }
        public int AcikHayvanHastalik { get; set; }

        // Mandıra
        public decimal BuHaftaSut { get; set; }
        public decimal BuAySut { get; set; }
        public List<SutGrafikDto> HaftalikSutGrafik { get; set; } = [];

        // Toptancı
        public int AktifTemin { get; set; }
        public decimal ToplamPlanlananMiktar { get; set; }

        // Tedarikçi
        public int KritikStok { get; set; }
        public int ToplamStok { get; set; }
    }

    public class YaklasanIslemDto
    {
        public string EkinTuru { get; set; } = string.Empty;
        public string AraziAdi { get; set; } = string.Empty;
        public string IslemTuru { get; set; } = string.Empty;
        public DateTime PlanlananTarih { get; set; }
    }

    public class SutGrafikDto
    {
        public string Gun { get; set; } = string.Empty;
        public decimal Miktar { get; set; }
    }
}
