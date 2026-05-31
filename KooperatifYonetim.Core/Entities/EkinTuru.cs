using KooperatifYonetim.Core.Enums;

namespace KooperatifYonetim.Core.Entities
{
    public class EkinTuru
    {
        public int EkinTuruId { get; set; }
        public string Ad { get; set; } = string.Empty;
        public ToplamaTipi ToplamaTipi { get; set; }
        public bool AktifMi { get; set; } = true;

        public ICollection<Ekin> Ekinler { get; set; } = new List<Ekin>();
    }
}
