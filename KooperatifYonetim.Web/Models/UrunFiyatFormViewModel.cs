using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooperatifYonetim.Web.Models
{
    public class UrunFiyatFormViewModel
    {
        public int? EkinTuruId { get; set; }

        [Display(Name = "Süt Fiyatı mı?")]
        public bool SutMu { get; set; }

        [Required(ErrorMessage = "Birim fiyat zorunludur.")]
        [Range(0.01, 999999)]
        [Display(Name = "Birim Fiyat (₺/kg veya ₺/litre)")]
        public decimal BirimFiyat { get; set; }

        [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
        [DataType(DataType.Date)]
        [Display(Name = "Başlangıç Tarihi")]
        public DateTime BaslangicTarihi { get; set; } = DateTime.Today;

        public IEnumerable<SelectListItem> EkinTuruListesi { get; set; } = [];
    }
}
