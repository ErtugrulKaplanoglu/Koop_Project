using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooperatifYonetim.Web.Models
{
    public class YemTedarikFormViewModel
    {
        [Required]
        [Display(Name = "Ahır")]
        public int AhirId { get; set; }

        [Required(ErrorMessage = "Yem türü zorunludur.")]
        [Display(Name = "Yem Türü")]
        public int YemTuruId { get; set; }

        [Required]
        [Range(0.01, 99999, ErrorMessage = "Talep miktarı 0'dan büyük olmalıdır.")]
        [Display(Name = "Talep Miktarı")]
        public decimal TalepMiktar { get; set; }

        [Display(Name = "Açıklama")]
        public string? Aciklama { get; set; }

        public IEnumerable<SelectListItem> AhirListesi { get; set; } = [];
        public IEnumerable<SelectListItem> YemTuruListesi { get; set; } = [];
    }
}
