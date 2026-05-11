using System.ComponentModel.DataAnnotations;
using KooperatifYonetim.Core.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooperatifYonetim.Web.Models
{
    public class VeterinerBakimFormViewModel
    {
        [Required]
        [Display(Name = "Ahır")]
        public int AhirId { get; set; }

        [Required]
        [Display(Name = "Veteriner")]
        public string VeterinerId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Planlanan Tarih")]
        public DateTime PlanlananTarih { get; set; } = DateTime.Today.AddDays(1);

        [Display(Name = "Bakım Türü")]
        public BakimTuru BakimTuru { get; set; }

        [Display(Name = "Notlar")]
        public string? Notlar { get; set; }

        public IEnumerable<SelectListItem> AhirListesi { get; set; } = [];
        public IEnumerable<SelectListItem> VeterinerListesi { get; set; } = [];
    }

    public class VeterinerBakimTamamlaViewModel
    {
        public int BakimId { get; set; }
        public string AhirAdi { get; set; } = string.Empty;
        public BakimTuru BakimTuru { get; set; }
        public DateTime PlanlananTarih { get; set; }

        [Display(Name = "Gerçekleşen Tarih")]
        public DateTime GerceklesenTarih { get; set; } = DateTime.Today;

        [Display(Name = "Notlar")]
        public string? Notlar { get; set; }
    }
}
