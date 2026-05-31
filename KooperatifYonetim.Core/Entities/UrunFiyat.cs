namespace KooperatifYonetim.Core.Entities
{
    public class UrunFiyat
    {
        public int UrunFiyatId { get; set; }
        public int? EkinTuruId { get; set; }
        public bool SutMu { get; set; }
        public decimal BirimFiyat { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }

        public EkinTuru? EkinTuru { get; set; }
    }
}
