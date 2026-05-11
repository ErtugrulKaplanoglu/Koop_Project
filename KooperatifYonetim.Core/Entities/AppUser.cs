using Microsoft.AspNetCore.Identity;

namespace KooperatifYonetim.Core.Entities
{
    public class AppUser : IdentityUser
    {
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string? Telefon { get; set; }

        public ICollection<Arazi> Araziler { get; set; } = new List<Arazi>();
        public ICollection<Ahir> Ahirlar { get; set; } = new List<Ahir>();
    }

    public class AppRole : IdentityRole
    {
        public string? Aciklama { get; set; }
    }
}
