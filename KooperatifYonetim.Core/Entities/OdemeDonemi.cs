using KooperatifYonetim.Core.Enums;

namespace KooperatifYonetim.Core.Entities
{
    public class OdemeDonemi
    {
        public int OdemeDonemiId { get; set; }
        public int Yil { get; set; }
        public int Ay { get; set; }
        public OdemeDonemiDurum Durum { get; set; } = OdemeDonemiDurum.Acik;
        public DateTime? OnayTarihi { get; set; }

        public ICollection<UreticiOdeme> UreticiOdemeler { get; set; } = new List<UreticiOdeme>();
    }
}
