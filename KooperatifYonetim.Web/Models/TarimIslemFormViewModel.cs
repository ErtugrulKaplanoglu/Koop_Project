using KooperatifYonetim.Core.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace KooperatifYonetim.Web.Models
{
    public class TarimIslemFormViewModel
    {
        public int IslemId { get; set; }

        [Required(ErrorMessage = "Ekin seçimi zorunludur.")]
        [Display(Name = "Ekin")]
        public int EkinId { get; set; }

        [Required(ErrorMessage = "İşlem türü zorunludur.")]
        [Display(Name = "İşlem Türü")]
        public IslemTuru IslemTuru { get; set; }

        [Required(ErrorMessage = "Planlanan tarih zorunludur.")]
        [DataType(DataType.Date)]
        [Display(Name = "Planlanan Tarih")]
        public DateTime PlanlananTarih { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        [Display(Name = "Gerçekleşen Tarih")]
        public DateTime? GerceklesenTarih { get; set; }

        [Display(Name = "Miktar")]
        public decimal? Miktar { get; set; }

        [Display(Name = "Notlar")]
        public string? Notlar { get; set; }

        public bool Tamamlandi { get; set; }

        public IEnumerable<SelectListItem> EkinListesi { get; set; } = Enumerable.Empty<SelectListItem>();
    }

    public class TamamlaViewModel
    {
        public int IslemId { get; set; }
        public string IslemTuruAdi { get; set; } = string.Empty;
        public DateTime PlanlananTarih { get; set; }

        [Required(ErrorMessage = "Gerçekleşen tarih zorunludur.")]
        [DataType(DataType.Date)]
        [Display(Name = "Gerçekleşen Tarih")]
        public DateTime GerceklesenTarih { get; set; } = DateTime.Today;

        [Display(Name = "Miktar (kg / litre)")]
        public decimal? Miktar { get; set; }
    }
}
