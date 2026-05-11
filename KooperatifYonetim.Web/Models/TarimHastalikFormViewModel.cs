using KooperatifYonetim.Core.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace KooperatifYonetim.Web.Models
{
    public class TarimHastalikFormViewModel
    {
        public int BildirimId { get; set; }

        [Required(ErrorMessage = "Ekin seçimi zorunludur.")]
        [Display(Name = "Ekin")]
        public int EkinId { get; set; }

        [Display(Name = "Ziraat Mühendisi")]
        public string? MuhendisId { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [Display(Name = "Hastalık / Sorun Açıklaması")]
        [MinLength(10, ErrorMessage = "En az 10 karakter giriniz.")]
        public string Aciklama { get; set; } = string.Empty;

        public IEnumerable<SelectListItem> EkinListesi { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> MuhendisListesi { get; set; } = Enumerable.Empty<SelectListItem>();
    }

    public class DurumGuncelleViewModel
    {
        public int BildirimId { get; set; }
        public string EkinTuru { get; set; } = string.Empty;
        public string AraziAdi { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
        public BildirimDurum MevcutDurum { get; set; }

        [Required]
        [Display(Name = "Yeni Durum")]
        public BildirimDurum YeniDurum { get; set; }
    }
}
