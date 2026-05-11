using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooperatifYonetim.Web.Models
{
    public class SutUretimiFormViewModel
    {
        [Required]
        [Display(Name = "Ahır")]
        public int AhirId { get; set; }

        [Required]
        [Display(Name = "Mandira")]
        public string MandiraId { get; set; } = string.Empty;

        [Range(0.01, 99999)]
        [Display(Name = "Miktar (litre)")]
        public decimal Miktar { get; set; }

        [Display(Name = "Tarih")]
        public DateTime Tarih { get; set; } = DateTime.Today;

        public IEnumerable<SelectListItem> AhirListesi { get; set; } = [];
        public IEnumerable<SelectListItem> MandiraListesi { get; set; } = [];
    }
}
