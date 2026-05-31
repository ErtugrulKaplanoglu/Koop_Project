# Kooperatif Yönetim Sistemi — Geliştirme Raporu

> ASP.NET Core MVC · .NET 9 · EF Core 9 · SQL Server LocalDB  
> Proje başlangıcından son düzenlemeye kadar yapılan tüm çalışmaların özeti.

---

## Hafta 1 / Gün 1 — Solution ve Proje Kurulumu

PLAN.md'deki mimariye uygun 4 katmanlı solution oluşturuldu:

- `KooperatifYonetim.Core` — entity, interface, enum, DTO sınıfları
- `KooperatifYonetim.Data` — AppDbContext, migration, seed
- `KooperatifYonetim.Business` — servis sınıfları, iş mantığı
- `KooperatifYonetim.Web` — MVC controller, view, wwwroot

Proje referansları `Web → Business → Data → Core` yönünde kuruldu. NuGet paketleri eklendi: `EF Core SQL Server`, `Identity.EntityFrameworkCore`, `Newtonsoft.Json`. `.NET 9 SDK` ve `dotnet-ef 9.0.4` kullanıldı (PLAN.md'deki .NET 8 yerine).

---

## Hafta 1 / Gün 2 — Core Katmanı: Entity ve Enum'lar

Tüm domain entity sınıfları `KooperatifYonetim.Core/Entities` altında oluşturuldu:

**Kullanıcı:** `AppUser` (IdentityUser'dan türer — Ad, Soyad, Telefon), `AppRole` (IdentityRole'dan türer — Aciklama)

**Tarım:** `Arazi` (AraziId, Ad, Enlem, Boylam, YuzOlcumu, UreticiId, AktifMi), `Ekin` (EkinId, AraziId, EkinTuru, EkimTarihi, HasatTarihi, Durum), `TarimIslem` (IslemId, EkinId, IslemTuru, PlanlananTarih, GerceklesenTarih, Miktar, Notlar, Tamamlandi), `UrunTemin` (ToptanciId, AraziId, Donem, PlanlananMiktar, AlinanMiktar), `TarimHastalikBildirimi` (EkinId, UreticiId, MuhendisId, Aciklama, Durum)

**Hayvancılık:** `Ahir` (AhirId, Ad, Adres, HayvanSayisi, UreticiId, AktifMi), `BesiStok` (AhirId, BesiTuru, MevcutMiktar, EsikMiktar, SonGuncelleme), `GunlukBesiGirisi` (AhirId, BesiTuru, YedirildenMiktar, Tarih), `SutUretimi` (AhirId, MandiraId, Miktar, Tarih), `VeterinerBakim` (AhirId, VeterinerId, PlanlananTarih, BakimTuru, Notlar, Tamamlandi), `HayvanHastalikBildirimi` (AhirId, UreticiId, VeterinerId, Aciklama, Durum)

**Enum'lar:** `EkinDurum`, `IslemTuru`, `BildirimDurum`, `BakimTuru`

---

## Hafta 1 / Gün 3 — Data Katmanı: DbContext ve Migration

`AppDbContext` : `IdentityDbContext<AppUser, AppRole, string>` olarak tanımlandı. Tüm entity'ler için `DbSet<>` özellikleri eklendi. `OnModelCreating`'de decimal alanlarına `HasPrecision(10,2)` ve FK ilişkilerine `OnDelete(DeleteBehavior.Restrict/Cascade)` konfigürasyonları uygulandı. `InitialCreate` migration'ı oluşturulup `KooperatifYonetimDb` (LocalDB) veritabanına uygulandı.

---

## Hafta 1 / Gün 4 — Identity: Rol Tanımları ve Seed Verisi

7 rol tanımlandı: `Yonetici`, `Uretici`, `Veteriner`, `ZiraatMuhendisi`, `Mandira`, `Toptanci`, `Tedarikci`. `SeedData` sınıfı oluşturuldu: roller, her rol için bir test kullanıcısı (şifre: `Sifre123!`) ve ekin/yem tür seed kayıtları `Program.cs`'teki migration akışına eklendi. Giriş (`/Account/Login`) ve çıkış (`/Account/Logout`) sayfaları oluşturuldu.

---

## Hafta 1 / Gün 5 — Web Layout, Sidebar ve Dashboard İskeleti

`_Layout.cshtml` oluşturuldu: Bootstrap 5 navbar (logo, rol badge, kullanıcı adı, çıkış), sol sabit sidebar, ana içerik alanı. `_SidebarMenu.cshtml` ile her rol için ayrı menü yapısı kuruldu. Özel CSS (`site.css`) ile yeşil tema, sidebar stilleri ve istatistik kart stilleri (`koop-stat-card`) tanımlandı. `InvariantDoubleModelBinder` eklenerek Türkçe yerel ayarında ondalık separator (virgül) sorunu giderildi. 7 rol için ayrı veri döndüren `DashboardService` ve `DashboardController` yazıldı.

---

## Hafta 2 / Gün 6 — Arazi Servisi ve Controller'ı

`IAraziService` interface'i ve `AraziService` implementasyonu oluşturuldu. `GetListeAsync` üretici ID ve admin bayrağına göre filtreliyor; `GetByIdAsync` Uretici ve Ekin navigasyon property'lerini include ediyor. `AraziController` yazıldı: `Index`, `Create` (GET/POST), `Edit` (GET/POST), `Delete` (POST) aksiyonları; rol bazlı yetkilendirme (`Yonetici`, `Uretici`).

---

## Hafta 2 / Gün 7 — Arazi View'ları

`Arazi/Index.cshtml`: tablo liste, arama butonu, üretici adı kolonu (yönetici görünümünde). `Arazi/Create.cshtml` ve `Arazi/Edit.cshtml`: enlem/boylam, yüz ölçümü ve ada sahip Bootstrap form. `Arazi/Details.cshtml`: koordinat bilgisi, ilgili ekin ve ürün teminlerinin alt tabloları.

---

## Hafta 2 / Gün 8 — Leaflet.js ve Hava Durumu Entegrasyonu

Arazi detay sayfasına Leaflet.js harita eklendi: koordinat üzerinde işaretçi pin ve popup. `IWeatherService` / `WeatherService` yazıldı: `HttpClient` ile OpenWeatherMap API çağrısı, `IMemoryCache` ile günlük önbellekleme. Arazi detay sayfasına sıcaklık, nem, hava durumu açıklaması ve ikon gösteren hava durumu kartı eklendi.

---

## Hafta 2 / Gün 9 — Ekin Servisi ve View'ları

`IEkinService` / `EkinService` oluşturuldu. `EkinController`: `Index`, `Create` (GET/POST), `Edit` (GET/POST), `DurumGuncelle` (POST) aksiyonları. `EkinFormViewModel` ile arazi dropdown. `Ekin/Index.cshtml`: durum badge'leri (Aktif/Hasat Aşaması/Tamamlandı). `Ekin/Create.cshtml` ve `Edit.cshtml`: arazi seçim dropdown'ı. Arazi detay sayfasına Ekinler alt tablosu eklendi.

---

## Hafta 2 / Gün 10 — Tarım İşlem Takibi

`ITarimIslemService` / `TarimIslemService` yazıldı. `TarimIslemController`: `Index`, `Create` (GET/POST), `Tamamla` (POST) aksiyonları. Sulama, ilaçlama, toplama işlem türleri; planlanan ve gerçekleşen tarih, miktar alanları. `TarimIslem/Index.cshtml`: bu hafta / geçmiş filtresi, tamamlandı checkbox. TarimIslemService'e **TekSefer Toplama kısıtı** eklendi: aynı ekine ikinci kez Toplama işlemi girilmesi engellendi.

---

## Hafta 3 / Gün 11–12 — Ürün Temin Yönetimi

`IUrunTeminService` / `UrunTeminService` oluşturuldu. `UrunTeminController`: toptancı kendi teminlerini, yönetici tümünü görür; `AlinanMiktar` güncelleme aksiyonu ayrıca eklendi. `UrunTemin/Index.cshtml`: dönem, planlanan / alınan miktar, toptancı adı. Arazi detay sayfasına temin alt tablosu eklendi.

---

## Hafta 3 / Gün 13–15 — Tarım Hastalık Bildirimi

`ITarimHastalikService` / `TarimHastalikService` yazıldı. `TarimHastalikController`: üretici ekin seçerek bildirim oluşturur; ziraat mühendisi kendi üzerine düşen (veya atanmamış) bildirimleri görür; `DurumGuncelle` (POST) ile mühendis üstlenir ve durumu günceller. `TarimHastalik/Index.cshtml`: durum badge'leri, atanan mühendis bilgisi. `Create.cshtml` ve durum güncelleme formu.

---

## Hafta 4 / Gün 16 — Ahır Yönetimi

`IAhirService` / `AhirService` yazıldı. `AhirController`: `Index`, `Create`, `Edit`, `Details` aksiyonları. `Ahir/Details.cshtml`: besi stok durumu, süt kayıtları, bakım takvimi ve hayvan hastalık bildirimleri alt tabloları. Yönetici tüm ahırları, üretici kendi ahırlarını görür.

---

## Hafta 4 / Gün 17 — Besi Stok ve Günlük Besi Girişi

`IBesiStokService` / `BesiStokService` oluşturuldu. `BesiStokController`: `Index`, `Create`, `Edit` aksiyonları. `BesiStok/Index.cshtml`: mevcut/eşik miktar progress bar, kırmızı "Kritik" badge. `IGunlukBesiGirisiService` / `GunlukBesiGirisiService` yazıldı. `BesiGirisController`: `Index`, `Create` (POST). Günlük giriş kaydedilince stok miktarı otomatik güncellendi.

---

## Hafta 4 / Gün 18–19 — Süt Üretimi

`ISutUretimiService` / `SutUretimiService` oluşturuldu. `SutUretimiController`: mandıra ve üretici ayrı görünümler; `Create` (POST) aksiyonu. `SutUretimi/Index.cshtml`: tarih, ahır, mandıra, miktar kolonu. Mandıra dashboard'una Chart.js ile son 7 günlük süt üretimi çubuk grafiği eklendi.

---

## Hafta 4 / Gün 20 — Hayvan Hastalık Bildirimi

`IHayvanHastalikService` / `HayvanHastalikService` oluşturuldu. `HayvanHastalikController`: üretici ahır seçip açıklama girer; veteriner kendi üzerine atanmış veya atanmamış bildirimleri görür; `DurumGuncelle` ile veteriner üstlenir. `HayvanHastalik/Index.cshtml`: durum badge'leri, atanan veteriner bilgisi.

---

## Hafta 5 / Gün 21–22 — Veteriner Bakım Takvimi

`IVeterinerBakimService` / `VeterinerBakimService` yazıldı. `VeterinerBakimController`: `Index`, `Create` (GET/POST), `Tamamla` (POST) aksiyonları. `VeterinerBakim/Index.cshtml`: bu hafta yaklaşan ve gecikmiş bakımlar ayrı bölümlerde gösterildi. Bakım türü (Periyodik / Acil) badge'leri, tamamlandı işaretleme butonu.

---

## Hafta 5 / Gün 23–25 — Tüm Rol Dashboard'ları

`DashboardService`'e tüm roller için ayrı metotlar eklendi. `DashboardController` rol tespiti yaparak ilgili metodu çağırır. Her rol için özelleştirilmiş özet kartlar:

- **Yönetici:** Toplam arazi, ahır, kullanıcı sayısı, açık bildirim sayısı + hızlı erişim butonları
- **Üretici:** Arazi/ahır sayısı, bu haftaki işlemler, açık bildirimler + yaklaşan işlemler tablosu
- **Ziraat Mühendisi:** Bekleyen / inceleniyor / çözüldü bildirim sayıları
- **Veteriner:** Bu hafta bakımlar, gecikmiş bakımlar, açık hayvan hastalıkları
- **Mandıra:** Bu hafta / bu ay süt toplamı + Chart.js haftalık grafik
- **Toptancı:** Aktif temin sayısı, planlanan toplam miktar
- **Tedarikçi:** Toplam stok kaydı, kritik seviyedeki stok sayısı

---

## Kullanıcı Yönetimi

`KullaniciController` oluşturuldu (yalnızca Yönetici). `UserManager` ile kullanıcı listeleme, yeni kullanıcı oluşturma (e-posta, şifre, rol seçimi), rol değiştirme aksiyonları. `Kullanici/Index.cshtml`: e-posta, aktif rol, işlem butonları. `Kullanici/Create.cshtml`: rol seçimli kullanıcı ekleme formu.

---

## Hafta 6 — Test Sonrası Düzeltmeler: Arazi Form Sorunları

Tüm haftalık geliştirme tamamlandıktan sonra manuel test sırasında arazi ekleme (`Create`) ve düzenleme (`Edit`) sayfalarında üç ayrı sorun tespit edildi.

**1 — Türkçe ondalık ayraç sorunu (`InvariantDoubleModelBinder`)**  
`Enlem` ve `Boylam` alanları `double` tipinde. Türkçe işletim sistemi / tarayıcı ortamında form değerleri sunucuya virgülle (`39,9104`) gönderildiğinden ASP.NET Core'un varsayılan model binder'ı parse edemiyordu; kayıt `0.0` ile kaydoluyor ya da doğrulama hatası dönüyordu. `InvariantDoubleModelBinder` sınıfı oluşturuldu — hem nokta hem virgülü ondalık ayraç olarak kabul edip `InvariantCulture` ile parse ediyor. `InvariantDoubleModelBinderProvider` ile tüm `double` / `double?` alanlarına otomatik uygulanacak şekilde `Program.cs`'e `options.ModelBinderProviders.Insert(0, ...)` kaydı yapıldı.

**2 — DMS (Derece-Dakika-Saniye) format desteği**  
Koordinatlar Google Maps'ten kopyalandığında `39°54'37.5"N` biçiminde geliyordu; bu format doğrudan `double.Parse`'a verilemediğinden hata üretiyordu. `Create.cshtml`'e `dmsToDec()` JavaScript fonksiyonu eklendi: hem `39.9104` (ondalık) hem `39°54'37.5"N` (DMS) formatını algılayıp ondalığa dönüştürüyor. Fonksiyon `blur` olayında ve form `submit` öncesinde çalışıyor; placeholder metni her iki format örneğini gösteriyor.

**3 — Haritadan koordinat seçimi**  
Koordinatları elle yazmak hata riskini artırıyordu. Arazi ekleme / düzenleme sayfasına sağ sütuna Leaflet.js haritası eklendi: haritaya tıklanınca `lat/lng` değerleri otomatik olarak `Enlem` ve `Boylam` input'larına yazılıyor, haritada marker gösteriliyor. Edit sayfasında kayıtlı koordinatlar sayfa açılışında haritada marker olarak önceden gösteriliyor.

---

## Blok Geliştirme 1 — Ekin Türü Yönetimi

`ToplamaTipi` enum'u ve `EkinTuru` entity'si oluşturuldu (EkinTuruId, Ad, ToplamaTipi, AktifMi). `Ekin.EkinTuru` string alanı kaldırılıp `EkinTuruId` FK alanına geçildi; `EkinTuruNavigation` navigasyon property'si eklendi. `IEkinTuruService` / `EkinTuruService` yazıldı. `EkinTuruController` ve CRUD view'ları (Index, Create, Edit) oluşturuldu. `EkinFormViewModel` güncellenerek ekin türü dropdown'a taşındı. `TarimIslemService`'e TekSefer Toplama kısıtı kesinleştirildi. `DashboardService`, `TarimIslemController` ve `TarimHastalikController`'daki `EkinTuru` string referansları `EkinTuruNavigation?.Ad` ile güncellendi. Migration: `AddEkinYemTurleri`.

---

## Blok Geliştirme 2 — Yem Türü Yönetimi

`YemBirim` enum'u (Balya, Cuval) ve `YemTuru` entity'si oluşturuldu (YemTuruId, Ad, Birim, AktifMi). `BesiStok.BesiTuru` ve `GunlukBesiGirisi.BesiTuru` string alanları kaldırılıp `YemTuruId` FK'ya geçildi. `IYemTuruService` / `YemTuruService` yazıldı. `YemTuruController` ve CRUD view'ları oluşturuldu. `BesiStokFormViewModel` ve `GunlukBesiGirisiFormViewModel` YemTuruId dropdown'ına taşındı. Birim etiketi (balya/çuval) view'larda JS ile `ViewBag.YemBirimMap`'ten dinamik okunacak şekilde ayarlandı. `BesiStokService`'deki eski `BesiTuru` referansları `YemTuruId` ve `YemTuru?.Ad` ile güncellendi. `SeedYemTurleri` seed metodu eklendi. Migration: `AddEkinYemTurleri` (ekin ve yem türleri aynı migration'a dahil edildi).

---

## Blok Geliştirme 3 — Aylık Satış ve Ödeme Sistemi

`UrunFiyat` entity'si oluşturuldu (EkinTuruId, BirimFiyat, GecerlilikBaslangic, GecerlilikBitis). `OdemeDonemi` entity'si oluşturuldu (Yil, Ay benzersiz index, Durum, HesaplamaTarihi). `UreticiOdeme` entity'si oluşturuldu (OdemeDonemiId, UreticiId, ToplamEkinKg, ToplamSutLitre, ToplamTutar, OdemeDetay JSON). `IUrunFiyatService` / `UrunFiyatService`, `IOdemeDonemiService` / `OdemeDonemiService` yazıldı. Ödeme hesaplama algoritması: üretici için o dönemdeki toplama işlemlerinden ekin kg ve süt litre toplamı alınıp `UrunFiyat` tablosundan birim fiyatlarla çarpılıp `ToplamTutar` hesaplanır; kalem detayları JSON olarak `OdemeDetay` alanına yazılır. `FiyatController` (ürün fiyat CRUD) ve `OdemeDonemiController` (dönem listesi, hesaplama tetikleyici, üretici geçmiş görünümü) oluşturuldu. Migration: `AddOdemeSistemi`.

---

## Aşama 1 — Veritabanı Yeniden Yapılandırması

**Yeni entity'ler:**

- `Bildirim` — AliciId (FK→AppUser, Cascade), Baslik, Mesaj, BildirimTipi enum, OlusturmaTarihi, Okundu bool, IlgiliKayitId int?
- `BesiHareketi` — AhirId, YemTuruId, HareketTipi enum (StokEkleme/Tuketim), Miktar, Tarih, Notlar?
- `YemTedarikBasvuru` — AhirId, UreticiId, YemTuruId, TalepMiktar, Aciklama?, BasvuruTarihi, Durum (BasvuruDurum enum), YoneticiNotu?

**Yeni enum'lar:** `BildirimTipi` (BesiStok, HayvanHastalik, TarimHastalik), `HareketTipi` (StokEkleme, Tuketim), `BasvuruDurum` (Beklemede, Inceleniyor, Tamamlandi)

**GunlukBesiGirisi kaldırıldı:** `AppDbContext`'ten DbSet silindi; `GunlukBesiGirisiService` derleme hatası vermemesi için no-op stub'a dönüştürüldü; `BesiGirisController` `/BesiStok`'a yönlendirecek şekilde yeniden yazıldı; `Program.cs`'ten servis kaydı kaldırıldı; `Ahir` entity'sindeki `GunlukBesiGirisler` navigasyonu `BesiHareketleri` ve `YemTedarikBasvurular` ile değiştirildi; `AhirService`'teki ilgili `.Include()` güncellendi.

`AppDbContext` yeni DbSet'ler, precision ve FK konfigürasyonları ile güncellendi; `WithMany()` çağrıları Ahir navigasyon koleksiyonlarına bağlandı (shadow FK uyarısını giderdi).

Migration'lar oluşturulup uygulandı: `AddBildirimVeHareketSistemi`, `DropGunlukBesiGirisi`, `FixShadowFKs`.

---

## Aşama 2 — BesiStok Servis ve Controller Yeniden Yapılandırması

**`IBildirimService` / `BildirimService`** oluşturuldu: `GetKullaniciBildirimleriAsync`, `GetOkunmamisSayiAsync`, `OkunduIsaretleAsync`, `TumunuOkunduIsaretleAsync`, `CreateAsync`.

**`IBesiStokService`** yeni metotlarla genişletildi, **`BesiStokService`** yeniden yazıldı:

- `StokEkleAsync` — BesiHareketi(StokEkleme) kaydı oluşturur, MevcutMiktar'ı artırır, eşik kontrolü yapar
- `TuketimGirAsync` — BesiHareketi(Tuketim) kaydı oluşturur, MevcutMiktar'ı düşürür (0'ın altına düşmez), eşik kontrolü yapar
- `EsikGuncelleAsync` — yalnızca EsikMiktar günceller
- `GetHareketlerAsync` — stoka ait son 50 hareketi döndürür
- **Eşik bildirimi (24h spam önleme):** MevcutMiktar ≤ EsikMiktar olduğunda, aynı stok için son 24 saat içinde okunmamış BesiStok bildirimi yoksa Ahir.UreticiId'ye otomatik `Bildirim` oluşturulur.

**`BesiStokController`** yeniden yazıldı; `StokEkle`, `TuketimGir`, `EsikGuncelle` (her biri GET/POST) aksiyonları eklendi. `BesiHareketiFormViewModel` ve `EsikGuncelleViewModel` oluşturuldu.

**View'lar:** `BesiStok/Index.cshtml` güncellendi (Kritik/Normal badge, progress bar, üç yeni aksiyon butonu); `StokEkle.cshtml`, `TuketimGir.cshtml`, `EsikGuncelle.cshtml` oluşturuldu.

---

## Aşama 3 — Bildirim Merkezi ve Yem Tedarik Başvurusu

**`IYemTedarikService` / `YemTedarikService`** oluşturuldu: başvuru listeleme, oluşturma (kayıt + tüm Yönetici kullanıcılara bildirim gönderme), durum güncelleme.

**`TarimHastalikService.CreateAsync`** güncellendi: atanan mühendis varsa `BildirimTipi.TarimHastalik` tipinde bildirim gönderilir.

**`HayvanHastalikService.CreateAsync`** güncellendi: atanan veteriner varsa `BildirimTipi.HayvanHastalik` tipinde bildirim gönderilir.

**`BildirimController`** oluşturuldu (`Authorize: Uretici,Yonetici,Veteriner,ZiraatMuhendisi`): `Index` (kullanıcının tüm bildirimleri, yeniden eskiye), `OkunduIsaretle` POST, `TumunuOku` POST. `Bildirim/Index.cshtml` view'ı oluşturuldu: bildirim türüne göre ikon ve renk, okunmamışlar vurgulu.

**`YemTedarikController`** oluşturuldu: `Index` (üretici kendi başvurularını, yönetici tümünü), `Create` (yalnızca Üretici), `DurumGuncelle` (yalnızca Yönetici). `YemTedarik/Index.cshtml`, `Create.cshtml`, `DurumGuncelle.cshtml` view'ları oluşturuldu. `YemTedarikFormViewModel` oluşturuldu.

`Program.cs`'e `IBildirimService` ve `IYemTedarikService` kayıtları eklendi.

---

## Aşama 4 — Layout'a Bildirim Entegrasyonu

**`BildirimZiliViewComponent`** oluşturuldu (`ViewComponents/BildirimZiliViewComponent.cs`): `IBildirimService.GetOkunmamisSayiAsync` ile okunmamış sayısını alır; `Views/Shared/Components/BildirimZili/Default.cshtml` view'ında zil simgesi ve sayı > 0 ise kırmızı badge render edilir.

**`_Layout.cshtml`** güncellendi: navbar'daki kullanıcı adı ile çıkış butonu arasına `@await Component.InvokeAsync("BildirimZili")` eklendi.

**`_SidebarMenu.cshtml`** güncellendi:
- Üretici: `BesiGiris` linki kaldırılıp `BesiStok`'a (Besi Yönetimi) değiştirildi; Yem Tedarik ve Bildirimlerim linkleri eklendi.
- Yönetici: Yem Tedarik ve Bildirimlerim linkleri eklendi.
- Ziraat Mühendisi ve Veteriner: Bildirimlerim linki eklendi.

---

## Hata Düzeltmesi — Dashboard Razor Render Sorunu

`Views/Dashboard/Index.cshtml` dosyasında iki ayrı Razor sözdizimi hatası giderildi:

**1. `@section Scripts` koşullu blok içindeydi:** Razor, `@section` direktiflerinin `if/else` bloğu içinde tanımlanmasını desteklemez; Mandıra rolüne ait Chart.js `@section Scripts` bloğu `else if (Mandira)` içine yazılmıştı, bu yüzden section kodu ham JavaScript metni olarak sayfaya basılıyordu. Düzeltme: `@section Scripts` bloğu dosyanın en altına (tüm `if/else` zincirinin dışına) taşındı; `@if (User.IsInRole("Mandira"))` koşulu ile sarıldı.

**2. HTML yorumları ve boş satırlar `else if` zincirini kırıyordu:** `}` ile `else if` arasına yerleştirilmiş `<!-- ===== BAŞLIK ===== -->` HTML yorumları ve boş satırlar, Razor'ın if/else zincirini parçalıyordu; `else if (User.IsInRole("..."))` ifadeleri Razor kodu olarak değil düz metin olarak sayfaya yazılıyordu. Düzeltme: Tüm `}` — `else if` / `else` geçişlerindeki HTML yorumları ve boş satırlar kaldırıldı, bloklar doğrudan bitiştirildi.
