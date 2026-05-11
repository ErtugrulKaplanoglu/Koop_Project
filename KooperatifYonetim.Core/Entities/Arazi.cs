namespace KooperatifYonetim.Core.Entities
{
    public class Arazi
    {
        public int AraziId { get; set; }
        public string Ad { get; set; } = string.Empty;
        public double Enlem { get; set; }
        public double Boylam { get; set; }
        public decimal YuzOlcumu { get; set; }
        public string UreticiId { get; set; } = string.Empty;
        public bool AktifMi { get; set; } = true;

        public AppUser Uretici { get; set; } = null!;
        public ICollection<Ekin> Ekinler { get; set; } = new List<Ekin>();
        public ICollection<UrunTemin> UrunTeminler { get; set; } = new List<UrunTemin>();
    }
}
