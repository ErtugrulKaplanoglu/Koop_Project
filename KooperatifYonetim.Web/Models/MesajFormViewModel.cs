using KooperatifYonetim.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace KooperatifYonetim.Web.Models
{
    public class MesajFormViewModel
    {
        [Required(ErrorMessage = "Alıcı seçiniz.")]
        public string AliciId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Konu giriniz.")]
        [MaxLength(200)]
        public string Konu { get; set; } = string.Empty;

        [Required(ErrorMessage = "İçerik giriniz.")]
        public string Icerik { get; set; } = string.Empty;

        public List<AppUser> IzinliAlicilar { get; set; } = new();
    }
}
