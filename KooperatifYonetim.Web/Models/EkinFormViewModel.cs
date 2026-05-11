using KooperatifYonetim.Core.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace KooperatifYonetim.Web.Models
{
    public class EkinFormViewModel
    {
        public int EkinId { get; set; }

        [Required(ErrorMessage = "Arazi seçimi zorunludur.")]
        [Display(Name = "Arazi")]
        public int AraziId { get; set; }

        [Required(ErrorMessage = "Ekin türü zorunludur.")]
        [Display(Name = "Ekin Türü")]
        public string EkinTuru { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ekim tarihi zorunludur.")]
        [DataType(DataType.Date)]
        [Display(Name = "Ekim Tarihi")]
        public DateTime EkimTarihi { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        [Display(Name = "Tahmini Hasat Tarihi")]
        public DateTime? HasatTarihi { get; set; }

        [Display(Name = "Durum")]
        public EkinDurum Durum { get; set; } = EkinDurum.Aktif;

        public IEnumerable<SelectListItem> AraziListesi { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
