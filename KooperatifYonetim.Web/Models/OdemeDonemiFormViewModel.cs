using System.ComponentModel.DataAnnotations;

namespace KooperatifYonetim.Web.Models
{
    public class OdemeDonemiFormViewModel
    {
        [Required]
        [Range(2020, 2100)]
        [Display(Name = "Yıl")]
        public int Yil { get; set; } = DateTime.Today.Year;

        [Required]
        [Range(1, 12)]
        [Display(Name = "Ay")]
        public int Ay { get; set; } = DateTime.Today.Month;
    }
}
