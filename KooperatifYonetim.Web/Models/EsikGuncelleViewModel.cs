using System.ComponentModel.DataAnnotations;

namespace KooperatifYonetim.Web.Models
{
    public class EsikGuncelleViewModel
    {
        public int StokId { get; set; }

        [Required(ErrorMessage = "Eşik miktar zorunludur.")]
        [Range(0, 99999)]
        [Display(Name = "Eşik Miktar")]
        public decimal EsikMiktar { get; set; }

        public string AhirAd { get; set; } = string.Empty;
        public string YemTuruAd { get; set; } = string.Empty;
        public string Birim { get; set; } = string.Empty;
        public decimal MevcutMiktar { get; set; }
    }
}
