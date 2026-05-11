using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooperatifYonetim.Web.Models
{
    public class AhirFormViewModel
    {
        public int AhirId { get; set; }

        [Required(ErrorMessage = "Ahır adı zorunludur.")]
        [Display(Name = "Ahır Adı")]
        public string Ad { get; set; } = string.Empty;

        [Display(Name = "Adres")]
        public string Adres { get; set; } = string.Empty;

        [Range(0, 10000)]
        [Display(Name = "Hayvan Sayısı")]
        public int HayvanSayisi { get; set; }

        [Required(ErrorMessage = "Üretici seçimi zorunludur.")]
        [Display(Name = "Üretici")]
        public string UreticiId { get; set; } = string.Empty;

        public IEnumerable<SelectListItem> UreticiListesi { get; set; } = [];
    }
}
