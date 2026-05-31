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

        [Required(ErrorMessage = "Yem türü zorunludur.")]
        [Display(Name = "Yem Türü")]
        public int YemTuruId { get; set; }

        [Range(0, 999999)]
        [Display(Name = "Mevcut Miktar")]
        public decimal MevcutMiktar { get; set; }

        [Range(0, 999999)]
        [Display(Name = "Eşik Miktar")]
        public decimal EsikMiktar { get; set; }

        public IEnumerable<SelectListItem> AhirListesi { get; set; } = [];
        public IEnumerable<SelectListItem> YemTuruListesi { get; set; } = [];
    }
}
