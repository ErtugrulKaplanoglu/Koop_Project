namespace KooperatifYonetim.Core.Enums
{
    public enum EkinDurum
    {
        Aktif = 0,
        HasatAsamasi = 1,
        Tamamlandi = 2
    }

    public enum IslemTuru
    {
        Sulama = 0,
        Ilacalama = 1,
        Toplama = 2
    }

    public enum BildirimDurum
    {
        Beklemede = 0,
        Inceleniyor = 1,
        Cozuldu = 2
    }

    public enum BakimTuru
    {
        Periyodik = 0,
        Acil = 1
    }

    public enum ToplamaTipi
    {
        Periyodik = 0,
        TekSefer = 1
    }

    public enum YemBirim
    {
        Balya = 0,
        Cuval = 1
    }

    public enum OdemeDonemiDurum
    {
        Acik = 0,
        Kesinlesti = 1
    }

    public enum BildirimTipi
    {
        BesiStok = 0,
        HayvanHastalik = 1,
        TarimHastalik = 2
    }

    public enum HareketTipi
    {
        StokEkleme = 0,
        Tuketim = 1
    }

    public enum BasvuruDurum
    {
        Beklemede = 0,
        Inceleniyor = 1,
        Tamamlandi = 2
    }
}
