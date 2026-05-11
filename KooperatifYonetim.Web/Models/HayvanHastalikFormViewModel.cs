using System.ComponentModel.DataAnnotations;
using KooperatifYonetim.Core.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooperatifYonetim.Web.Models
{
    public class HayvanHastalikFormViewModel
    {
        [Required]
        [Display(Name = "Ahır")]
        public int AhirId { get; set; }

        [Display(Name = "Veteriner")]
        public string? VeterinerId { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [Display(Name = "Açıklama")]
        public string Aciklama { get; set; } = string.Empty;

        public IEnumerable<SelectListItem> AhirListesi { get; set; } = [];
        public IEnumerable<SelectListItem> VeterinerListesi { get; set; } = [];
    }

    public class HayvanDurumGuncelleViewModel
    {
        public int BildirimId { get; set; }
        public string AhirAdi { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
        public BildirimDurum MevcutDurum { get; set; }

        [Display(Name = "Yeni Durum")]
        public BildirimDurum YeniDurum { get; set; }
    }
}
