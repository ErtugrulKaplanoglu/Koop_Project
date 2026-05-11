using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooperatifYonetim.Web.Models
{
    public class AraziFormViewModel
    {
        public int AraziId { get; set; }

        [Required(ErrorMessage = "Arazi adı zorunludur.")]
        [Display(Name = "Arazi Adı")]
        public string Ad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enlem zorunludur.")]
        [Range(-90, 90, ErrorMessage = "Geçerli bir enlem giriniz.")]
        [Display(Name = "Enlem (Latitude)")]
        public double Enlem { get; set; }

        [Required(ErrorMessage = "Boylam zorunludur.")]
        [Range(-180, 180, ErrorMessage = "Geçerli bir boylam giriniz.")]
        [Display(Name = "Boylam (Longitude)")]
        public double Boylam { get; set; }

        [Required(ErrorMessage = "Yüz ölçümü zorunludur.")]
        [Range(0.01, 999999, ErrorMessage = "Geçerli bir değer giriniz.")]
        [Display(Name = "Yüz Ölçümü (Dönüm)")]
        public decimal YuzOlcumu { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool AktifMi { get; set; } = true;

        [Required(ErrorMessage = "Üretici seçimi zorunludur.")]
        [Display(Name = "Üretici")]
        public string UreticiId { get; set; } = string.Empty;

        public IEnumerable<SelectListItem> UreticiListesi { get; set; } = [];
    }
}
