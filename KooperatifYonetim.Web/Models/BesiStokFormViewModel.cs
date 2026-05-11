using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooperatifYonetim.Web.Models
{
    public class BesiStokFormViewModel
    {
        public int StokId { get; set; }

        [Required]
        [Display(Name = "Ahır")]
        public int AhirId { get; set; }

        [Required(ErrorMessage = "Besi türü zorunludur.")]
        [Display(Name = "Besi Türü")]
        public string BesiTuru { get; set; } = string.Empty;

        [Range(0, 999999)]
        [Display(Name = "Mevcut Miktar (kg)")]
        public decimal MevcutMiktar { get; set; }

        [Range(0, 999999)]
        [Display(Name = "Eşik Miktar (kg)")]
        public decimal EsikMiktar { get; set; }

        public IEnumerable<SelectListItem> AhirListesi { get; set; } = [];
    }
}
