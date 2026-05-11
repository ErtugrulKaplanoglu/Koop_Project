using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooperatifYonetim.Web.Models
{
    public class KullaniciFormViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Ad zorunludur.")]
        [Display(Name = "Ad")]
        public string Ad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad zorunludur.")]
        [Display(Name = "Soyad")]
        public string Soyad { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Telefon")]
        public string? Telefon { get; set; }

        [Display(Name = "Rol")]
        [Required(ErrorMessage = "Rol seçimi zorunludur.")]
        public string Rol { get; set; } = string.Empty;

        [Display(Name = "Şifre")]
        [MinLength(8, ErrorMessage = "Şifre en az 8 karakter olmalıdır.")]
        public string? Sifre { get; set; }

        public string MevcutRol { get; set; } = string.Empty;

        public List<SelectListItem> RolListesi { get; set; } = [];
    }

    public class KullaniciListeViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefon { get; set; }
        public string Rol { get; set; } = string.Empty;
    }
}
