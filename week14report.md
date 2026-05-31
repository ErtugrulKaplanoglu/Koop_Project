# Hafta 15 Geliştirme Raporu

## MODÜL 1 — Raporlama Sistemi

### Amaç
Üretici, Yönetici ve Mandıra rollerine özel aylık istatistik raporları sunmak; Chart.js ile görsel grafikler eklemek.

### Oluşturulan / Güncellenen Dosyalar

#### Core Katmanı
- **`KooperatifYonetim.Core/DTOs/RaporDtos.cs`**
  - `UreticiAylikOzetDto` — sulama/ilaçlama/toplama sayıları, toplam hasat (kg), toplam süt (lt), açık hastalık bildirimleri
  - `YoneticiUreticiSatirDto` — üretici bazında hasat, süt, ödeme tutarı
  - `GunlukSutDto` — tarih ve günlük süt miktarı

- **`KooperatifYonetim.Core/Interfaces/IRaporService.cs`**
  - `GetUreticiAylikOzetAsync(ureticiId, yil, ay)` — üretici özet verisi
  - `GetYoneticiAylikOzetAsync(yil, ay)` — tüm üretici listesi
  - `GetMandiraAylikSutAsync(mandiraId, yil, ay)` — günlük süt listesi

#### Business Katmanı
- **`KooperatifYonetim.Business/Services/RaporService.cs`**
  - `GetUreticiAylikOzetAsync`: TarimIslemler üzerinden IslemTuru bazlı sayım; SutUretimleri toplamı; açık TarimHastalik ve HayvanHastalik bildirim sayısı
  - `GetYoneticiAylikOzetAsync`: `UserManager.GetUsersInRoleAsync("Uretici")` ile üretici listesi; OdemeDonemi tablosundan ilgili ay/yıl kaydı eşleştirilir
  - `GetMandiraAylikSutAsync`: SutUretimleri `GroupBy(Tarih.Date)` ile günlük toplam

#### Web Katmanı
- **`Models/RaporViewModel.cs`** — 3 ViewModel; `ToplamSut`, `ToplamHasatKg`, `ToplamOdeme` hesaplanan özellikler
- **`Controllers/RaporController.cs`** — `Index` (role göre yönlendirir), `UreticiRaporu`, `MandiraRaporu`, `YoneticiRaporu` action'ları
- **`Views/Rapor/UreticiRaporu.cshtml`** — hasat/süt/hastalık stat kartları + Chart.js **bar grafiği** (sulama/ilaçlama/toplama sayıları)
- **`Views/Rapor/MandiraRaporu.cshtml`** — toplam/ortalama/gün sayısı stat kartları + Chart.js **line grafiği** + günlük detay tablosu
- **`Views/Rapor/YoneticiRaporu.cshtml`** — genel toplam kartlar + Chart.js **grouped bar grafiği** (üretici bazlı hasat vs süt) + üretici detay tablosu
- **`Views/Shared/_SidebarMenu.cshtml`** — Yönetici: "Raporlar", Üretici: "Raporlarım", Mandıra: "Süt Raporları" linkleri eklendi
- **`Program.cs`** — `IRaporService → RaporService` kaydı eklendi

---

## MODÜL 2 — Mesajlaşma Sistemi

### Amaç
Roller arası dahili mesajlaşma altyapısı kurmak; izin matrisi ile kimin kime mesaj atabileceğini kısıtlamak; navbar'a mesaj zili eklemek.

### Oluşturulan / Güncellenen Dosyalar

#### Core Katmanı
- **`KooperatifYonetim.Core/Entities/Mesaj.cs`**
  - `[Key] MesajId`, `GonderenId`, `AliciId`, `Konu`, `Icerik`, `GonderimTarihi`, `OkunduMu`
  - `Gonderen` ve `Alici` navigasyon özellikleri (`AppUser`)

- **`KooperatifYonetim.Core/Interfaces/IMesajService.cs`**
  - `GetInboxAsync`, `GetGonderilenlerAsync`, `GetMesajDetayAsync`, `SendAsync`, `GetOkunmamisSayiAsync`, `GetIzinliAlicilarAsync`

#### Data Katmanı
- **`KooperatifYonetim.Data/AppDbContext.cs`** — `DbSet<Mesaj>` eklendi; `GonderenId` ve `AliciId` FK'ları `OnDelete(Restrict)` olarak yapılandırıldı
- **Migration: `AddMesajlasma`** — `Mesajlar` tablosu oluşturuldu

#### Business Katmanı
- **`KooperatifYonetim.Business/Services/MesajService.cs`**
  - `GetMesajDetayAsync`: Alıcı görüntülediğinde `OkunduMu = true` otomatik güncellenir
  - `GetIzinliAlicilarAsync`: İzin matrisi:
    | Gönderen | Mesaj Atabileceği Roller |
    |---|---|
    | Yönetici | Üretici, ZiraatMühendisi, Veteriner, Toptancı, Mandıra, Tedarikçi |
    | Üretici | Yönetici, ZiraatMühendisi, Veteriner |
    | ZiraatMühendisi | Yönetici, Üretici |
    | Veteriner | Yönetici, Üretici |
    | Mandıra / Toptancı / Tedarikçi | Yönetici |

#### Web Katmanı
- **`Models/MesajFormViewModel.cs`** — `AliciId`, `Konu`, `Icerik` (zorunlu alanlar) + `IzinliAlicilar` listesi
- **`Controllers/MesajController.cs`** — `Inbox`, `Gonderilenler`, `Detay`, `YeniMesaj` (GET/POST)
- **`ViewComponents/MesajZiliViewComponent.cs`** — okunmamış mesaj sayısını döner
- **`Views/Shared/Components/MesajZili/Default.cshtml`** — zarif ikonu, 99+ badge
- **`Views/Shared/_Layout.cshtml`** — `MesajZili` bileşeni navbar'a `BildirimZili` yanına eklendi
- **`Views/Mesaj/Inbox.cshtml`** — okunmamış mesajlar kalın; mavi nokta göstergesi
- **`Views/Mesaj/Gonderilenler.cshtml`** — okundu/okunmadı rozeti
- **`Views/Mesaj/Detay.cshtml`** — gönderen/alıcı/tarih başlık alanı + mesaj içeriği
- **`Views/Mesaj/YeniMesaj.cshtml`** — izinli alıcı dropdown + validasyonlu form
- **`Views/Shared/_SidebarMenu.cshtml`** — tüm 7 role "Mesajlarım" bağlantısı eklendi
- **`Program.cs`** — `IMesajService → MesajService` kaydı eklendi

---

## Build Durumu
`dotnet build` → **0 hata, 0 uyarı**
