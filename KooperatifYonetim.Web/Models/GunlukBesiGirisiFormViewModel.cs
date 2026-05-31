using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooperatifYonetim.Web.Models
{
    public class GunlukBesiGirisiFormViewModel
    {
        [Required]
        [Display(Name = "Ahır")]
        public int AhirId { get; set; }

        [Required(ErrorMessage = "Yem türü zorunludur.")]
        [Display(Name = "Yem Türü")]
        public int YemTuruId { get; set; }

        [Range(0.01, 99999)]
        [Display(Name = "Yedirilen Miktar")]
        public decimal YedirildenMiktar { get; set; }

        [Display(Name = "Tarih")]
        public DateTime Tarih { get; set; } = DateTime.Today;

        public IEnumerable<SelectListItem> AhirListesi { get; set; } = [];
        public IEnumerable<SelectListItem> YemTuruListesi { get; set; } = [];
    }
}
