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
}
