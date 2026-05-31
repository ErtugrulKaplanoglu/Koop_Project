using System.ComponentModel.DataAnnotations;

namespace KooperatifYonetim.Web.Models
{
    public class BesiHareketiFormViewModel
    {
        public int StokId { get; set; }

        [Required(ErrorMessage = "Miktar zorunludur.")]
        [Range(0.01, 99999, ErrorMessage = "Miktar 0'dan büyük olmalıdır.")]
        [Display(Name = "Miktar")]
        public decimal Miktar { get; set; }

        [Display(Name = "Notlar")]
        public string? Notlar { get; set; }

        public string AhirAd { get; set; } = string.Empty;
        public string YemTuruAd { get; set; } = string.Empty;
        public string Birim { get; set; } = string.Empty;
        public decimal MevcutMiktar { get; set; }
        public decimal EsikMiktar { get; set; }
    }
}
