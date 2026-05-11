using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooperatifYonetim.Web.Models
{
    public class GunlukBesiGirisiFormViewModel
    {
        [Required]
        [Display(Name = "Ahır")]
        public int AhirId { get; set; }

        [Required(ErrorMessage = "Besi türü zorunludur.")]
        [Display(Name = "Besi Türü")]
        public string BesiTuru { get; set; } = string.Empty;

        [Range(0.01, 99999)]
        [Display(Name = "Yedirilen Miktar (kg)")]
        public decimal YedirildenMiktar { get; set; }

        [Display(Name = "Tarih")]
        public DateTime Tarih { get; set; } = DateTime.Today;

        public IEnumerable<SelectListItem> AhirListesi { get; set; } = [];
    }
}
