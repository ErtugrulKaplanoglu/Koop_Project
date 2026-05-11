using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace KooperatifYonetim.Web.Models
{
    public class UrunTeminFormViewModel
    {
        public int UrunTeminId { get; set; }

        [Required(ErrorMessage = "Arazi seçimi zorunludur.")]
        [Display(Name = "Arazi")]
        public int AraziId { get; set; }

        [Required(ErrorMessage = "Dönem zorunludur.")]
        [Display(Name = "Dönem (örn: 2025-Yaz)")]
        public string Donem { get; set; } = string.Empty;

        [Required(ErrorMessage = "Planlanan miktar zorunludur.")]
        [Range(0.01, 999999, ErrorMessage = "Geçerli bir miktar giriniz.")]
        [Display(Name = "Planlanan Miktar (kg)")]
        public decimal PlanlananMiktar { get; set; }

        [Display(Name = "Alınan Miktar (kg)")]
        public decimal? AlinanMiktar { get; set; }

        public IEnumerable<SelectListItem> AraziListesi { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
