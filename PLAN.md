# Kooperatif Yönetim Sistemi — Proje Planı
> ASP.NET Core MVC | SQL Server | 6 Hafta | ~150 Saat

---

## İçindekiler
1. [Proje Genel Bakış](#1-proje-genel-bakış)
2. [Teknoloji Stack](#2-teknoloji-stack)
3. [Mimari Yapı](#3-mimari-yapı)
4. [Veritabanı Şeması](#4-veritabanı-şeması)
5. [Kullanıcı Rolleri ve Yetkiler](#5-kullanıcı-rolleri-ve-yetkiler)
6. [UI Tasarım Kararları](#6-ui-tasarım-kararları)
7. [Modüller ve İşlevler](#7-modüller-ve-işlevler)
8. [Haftalık Geliştirme Planı](#8-haftalık-geliştirme-planı)
9. [Kapsam Dışı (İlk Aşama)](#9-kapsam-dışı-ilk-aşama)
10. [Geliştirme Ortamı Kurulumu](#10-geliştirme-ortamı-kurulumu)

---

## 1. Proje Genel Bakış

Tarım ve hayvancılık kooperatifini yönetmek için çok-kullanıcılı, rol tabanlı bir web uygulaması. Sistem; arazi takibi, ekin yönetimi, ahır ve hayvan besleme takibi, süt üretimi kaydı, veteriner bakım takvimi ve ürün temin yönetimini kapsar.

**Kullanıcı Profilleri:** Üretici, Mandıra, Toptancı, Tedarikçi, Veteriner, Ziraat Mühendisi, Yönetici

---

## 2. Teknoloji Stack

### Backend
| Teknoloji | Versiyon | Amaç |
|---|---|---|
| ASP.NET Core MVC | .NET 8 | Ana framework |
| Entity Framework Core | 8.x | ORM, Code First |
| ASP.NET Core Identity | 8.x | Kimlik doğrulama ve rol yönetimi |
| AutoMapper | 13.x | Entity ↔ DTO dönüşümleri |
| SQL Server | LocalDB (geliştirme) | Veritabanı |

### Frontend
| Teknoloji | Amaç |
|---|---|
| Razor Views (.cshtml) | Sunucu taraflı HTML üretimi |
| Bootstrap 5.3 | Grid, bileşenler, responsive |
| Bootstrap Icons | İkon seti |
| Leaflet.js | Arazi harita ve koordinat görüntüleme |
| Chart.js | Süt ve besi stoku grafikleri |
| Vanilla JS | Form validasyon, minimal etkileşim |

### Dış API'ler
| API | Amaç | Not |
|---|---|---|
| OpenWeatherMap | Arazi bazlı hava durumu | Ücretsiz tier (1000 req/gün) |
| OpenStreetMap (Leaflet ile) | Harita tile'ları | Ücretsiz |

### NuGet Paketleri
```
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
Microsoft.AspNetCore.Identity.EntityFrameworkCore
AutoMapper.Extensions.Microsoft.DependencyInjection
Newtonsoft.Json
```

---

## 3. Mimari Yapı

### Solution Yapısı
```
KooperatifYonetim.sln
│
├── KooperatifYonetim.Core         → Entity'ler, DTO'lar, Interface'ler, Enum'lar
├── KooperatifYonetim.Data         → DbContext, Repository'ler, Migration'lar
├── KooperatifYonetim.Business     → Service'ler, iş mantığı, API entegrasyonları
└── KooperatifYonetim.Web          → MVC Controllers, Views, wwwroot
```

### Bağımlılık Yönü
```
Web → Business → Data → Core
```
Core hiçbir projeye bağımlı değildir.

### Katman Sorumlulukları

**Core**
- Tüm entity sınıfları (Arazi, Ekin, Ahir vb.)
- DTO sınıfları (View model'leri için)
- Interface tanımları (IRepository, IService vb.)
- Enum tanımları

**Data**
- `AppDbContext` (IdentityDbContext'ten türer)
- Generic ve spesifik Repository implementasyonları
- EF Core Migration'ları
- Seed verileri

**Business**
- Service sınıfları (AraziService, AhirService vb.)
- OpenWeatherMap API entegrasyonu
- İş kuralları ve validasyon

**Web**
- MVC Controller'lar
- Razor Views
- wwwroot (CSS, JS, lib)
- Layout ve partial view'lar

---

## 4. Veritabanı Şeması

### 4.1 Kimlik ve Kullanıcı Yönetimi

```
AppUser (IdentityUser'dan türer)
├── Ad                 (string)
├── Soyad              (string)
└── Telefon            (string?)

AppRole (IdentityRole'dan türer)
└── Aciklama           (string?)

Roller:
  - Yonetici
  - Uretici
  - Veteriner
  - ZiraatMuhendisi
  - Mandira
  - Toptanci
  - Tedarikci
```

### 4.2 Tarım Modülü

```
Arazi
├── AraziId            (int, PK)
├── Ad                 (string)
├── Enlem              (double)
├── Boylam             (double)
├── YuzOlcumu          (decimal)       -- dönüm cinsinden
├── UreticiId          (string, FK → AppUser)
└── AktifMi            (bool)

Ekin
├── EkinId             (int, PK)
├── AraziId            (int, FK → Arazi)
├── EkinTuru           (string)
├── EkimTarihi         (DateTime)
├── HasatTarihi        (DateTime?)
└── Durum              (enum: Aktif=0, HasatAsamasi=1, Tamamlandi=2)

TarimIslem
├── IslemId            (int, PK)
├── EkinId             (int, FK → Ekin)
├── IslemTuru          (enum: Sulama=0, Ilacalama=1, Toplama=2)
├── PlanlananTarih     (DateTime)
├── GerceklesenTarih   (DateTime?)
├── Miktar             (decimal?)      -- toplama: kg | sulama: litre
├── Notlar             (string?)
└── Tamamlandi         (bool)

UrunTemin
├── UrunTeminId        (int, PK)
├── AraziId            (int, FK → Arazi)
├── ToptanciId         (string, FK → AppUser)
├── Donem              (string)        -- örn: "2025-Yaz"
├── PlanlananMiktar    (decimal)
└── AlinanMiktar       (decimal?)

TarimHastalıkBildirimi
├── BildirimId         (int, PK)
├── EkinId             (int, FK → Ekin)
├── UreticiId          (string, FK → AppUser)
├── MuhendisId         (string, FK → AppUser)
├── Aciklama           (string)
├── BildirimTarihi     (DateTime)
└── Durum              (enum: Beklemede=0, Inceleniyor=1, Cozuldu=2)
```

### 4.3 Hayvancılık Modülü

```
Ahir
├── AhirId             (int, PK)
├── Ad                 (string)
├── Adres              (string)
├── HayvanSayisi       (int)
├── UreticiId          (string, FK → AppUser)
└── AktifMi            (bool)

BesiStok
├── StokId             (int, PK)
├── AhirId             (int, FK → Ahir)
├── BesiTuru           (string)
├── MevcutMiktar       (decimal)       -- kg
├── EsikMiktar         (decimal)       -- bu altına düşünce uyarı göstergesi
└── SonGuncelleme      (DateTime)

GunlukBesiGirisi
├── GirisId            (int, PK)
├── AhirId             (int, FK → Ahir)
├── BesiTuru           (string)
├── YedirildenMiktar   (decimal)       -- kg
└── Tarih              (DateTime)

SutUretimi
├── SutId              (int, PK)
├── AhirId             (int, FK → Ahir)
├── MandiraId          (string, FK → AppUser)
├── Miktar             (decimal)       -- litre
└── Tarih              (DateTime)

VeterinerBakim
├── BakimId            (int, PK)
├── AhirId             (int, FK → Ahir)
├── VeterinerId        (string, FK → AppUser)
├── PlanlananTarih     (DateTime)
├── GerceklesenTarih   (DateTime?)
├── BakimTuru          (enum: Periyodik=0, Acil=1)
├── Notlar             (string?)
└── Tamamlandi         (bool)

HayvanHastalıkBildirimi
├── BildirimId         (int, PK)
├── AhirId             (int, FK → Ahir)
├── UreticiId          (string, FK → AppUser)
├── VeterinerId        (string, FK → AppUser)
├── Aciklama           (string)
├── BildirimTarihi     (DateTime)
└── Durum              (enum: Beklemede=0, Inceleniyor=1, Cozuldu=2)
```

### 4.4 İlişki Özeti
```
AppUser      → Arazi[]            (1-N)
AppUser      → Ahir[]             (1-N)
Arazi        → Ekin[]             (1-N)
Ekin         → TarimIslem[]       (1-N)
Arazi        → UrunTemin[]        (1-N)
Ahir         → BesiStok[]         (1-N)
Ahir         → GunlukBesiGirisi[] (1-N)
Ahir         → SutUretimi[]       (1-N)
Ahir         → VeterinerBakim[]   (1-N)
```

---

## 5. Kullanıcı Rolleri ve Yetkiler

| Sayfa / İşlev | Yönetici | Üretici | Ziraat Müh. | Veteriner | Toptancı | Mandıra | Tedarikçi |
|---|:---:|:---:|:---:|:---:|:---:|:---:|:---:|
| Kullanıcı yönetimi | ✅ | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |
| Arazi CRUD | ✅ | ✅ | ❌ | ❌ | ❌ | ❌ | ❌ |
| Arazi görüntüleme | ✅ | ✅ | ✅ | ❌ | ✅ | ❌ | ❌ |
| Ekin yönetimi | ✅ | ✅ | ❌ | ❌ | ❌ | ❌ | ❌ |
| Tarım işlem kaydı | ✅ | ✅ | ❌ | ❌ | ❌ | ❌ | ❌ |
| Hastalık bildirimi (tarım) | ✅ | ✅ | ✅ | ❌ | ❌ | ❌ | ❌ |
| Ürün temin yönetimi | ✅ | ✅ | ❌ | ❌ | ✅ | ❌ | ❌ |
| Ahır CRUD | ✅ | ✅ | ❌ | ❌ | ❌ | ❌ | ❌ |
| Ahır görüntüleme | ✅ | ✅ | ❌ | ✅ | ❌ | ✅ | ✅ |
| Besi girişi | ✅ | ✅ | ❌ | ❌ | ❌ | ❌ | ❌ |
| Besi stok görüntüleme | ✅ | ✅ | ❌ | ❌ | ❌ | ❌ | ✅ |
| Süt kaydı | ✅ | ✅ | ❌ | ❌ | ❌ | ✅ | ❌ |
| Veteriner bakım takvimi | ✅ | ❌ | ❌ | ✅ | ❌ | ❌ | ❌ |
| Hastalık bildirimi (hayvan) | ✅ | ✅ | ❌ | ✅ | ❌ | ❌ | ❌ |
| Hava durumu görüntüleme | ✅ | ✅ | ✅ | ❌ | ❌ | ❌ | ❌ |

---

## 6. UI Tasarım Kararları

### Genel Ton
- Kurumsal ve sade, Bootstrap 5 tabanlı
- Karmaşık animasyon veya custom component yok
- Hızlı yükleme, basit navigasyon öncelikli

### Renk Paleti
```css
--color-primary:       #2E7D32;   /* Ana yeşil — sidebar, butonlar */
--color-primary-light: #66BB6A;   /* Hover, vurgu */
--color-bg:            #F5F5F5;   /* Sayfa zemini */
--color-card:          #FFFFFF;   /* Kart/panel arka planı */
--color-text:          #212121;   /* Ana metin */
--color-text-muted:    #757575;   /* İkincil metin */
--color-danger:        #C62828;   /* Silme, hata */
--color-warning:       #F57F17;   /* Stok uyarısı vb. */
```

### Layout
```
┌─────────────────────────────────────────────────┐
│  NAVBAR — Logo | Aktif Rol | Kullanıcı | Çıkış  │
├───────────────┬─────────────────────────────────┤
│               │                                 │
│   SIDEBAR     │         ANA İÇERİK              │
│  (role göre   │     (sayfa içeriği burada)      │
│   menü items) │                                 │
│               │                                 │
└───────────────┴─────────────────────────────────┘
```
Mobilde sidebar hamburger menüye dönüşür.

### Role Göre Sidebar Menüleri

**Yönetici:** Dashboard | Kullanıcı Yönetimi | Tüm Araziler | Tüm Ahırlar

**Üretici:** Dashboard | Arazilerim (Ekinler, Tarım İşlemleri, Hastalık Bildirimleri) | Ahırlarım (Besi Girişi, Süt Kaydı, Hastalık Bildirimleri)

**Ziraat Mühendisi:** Dashboard | Hastalık Bildirimleri | Arazi Listesi (görüntüleme)

**Veteriner:** Dashboard | Bakım Takvimim | Hastalık Bildirimleri | Ahır Listesi (görüntüleme)

**Toptancı:** Dashboard | Ürün Teminlerim | Arazi Listesi (görüntüleme)

**Mandıra:** Dashboard | Süt Kayıtları

**Tedarikçi:** Dashboard | Besi Stok Durumları

### Standart Sayfa Şablonları

**Liste Sayfası**
```
[Başlık]                              [+ Yeni Ekle]
──────────────────────────────────────────────────
[Arama] [Filtre]
Bootstrap Table (striped, hover)
  Sütunlar | ... | [Görüntüle] [Düzenle] [Sil]
[Sayfalama]
```

**Detay Sayfası**
```
[← Geri]                         [Düzenle] [Sil]
Kart: Temel bilgiler
Harita (arazi sayfalarında — Leaflet.js)
Alt Tablolar: İlgili kayıtlar
```

**Form Sayfası**
```
[Başlık]
2 sütunlu Bootstrap form
[İptal]                               [Kaydet]
```

### Dashboard Özet Kartları
Her dashboard 4 özet kart + 1-2 tablo içerir. Kartlar: toplam arazi/ahır sayısı, bu haftaki işlemler, açık bildirimler gibi özet verileri gösterir.

---

## 7. Modüller ve İşlevler

### 7.1 Kimlik Doğrulama Modülü
- Login / Logout sayfaları
- Rol bazlı yetkilendirme (`[Authorize(Roles = "...")]`)
- Yönetici: yeni kullanıcı oluşturma ve rol atama

### 7.2 Tarım Modülü

**Arazi Yönetimi**
- Arazi listesi (üreticinin kendi arazileri; yönetici hepsini görür)
- Arazi ekleme: ad, koordinat (enlem/boylam), yüz ölçümü
- Arazi detay sayfası: Leaflet.js haritada pin + hava durumu kartı + alt tablolar
- Düzenleme ve silme

**Ekin Yönetimi**
- Bir araziye bağlı ekin listesi
- Ekin ekleme: tür, ekim tarihi, tahmini hasat tarihi
- Durum güncelleme (Aktif → Hasat Aşaması → Tamamlandı)

**Tarım İşlem Takibi**
- Bir ekine bağlı işlem listesi (sulama, ilaçlama, toplama)
- İşlem ekleme: tür, planlanan tarih, miktar
- Gerçekleşme kaydı: fiili tarih ve miktar girişi
- Tamamlandı işaretleme

**Ürün Temin Yönetimi**
- Toptancı hangi araziden, hangi dönem, ne kadar ürün alacak → kayıt
- Alınan miktar sonradan güncellenir

**Tarım Hastalık Bildirimi**
- Üretici: ekin seçerek açıklama girer → ziraat mühendisine atanır
- Ziraat mühendisi: üstüne düşen bildirimleri görür, durumu günceller

**Hava Durumu**
- Arazi detay sayfasında koordinata göre OpenWeatherMap çağrısı
- Sıcaklık, hava durumu, nem gösterimi
- Veri aynı gün cache'lenir (gereksiz API çağrısı önlenir)

### 7.3 Hayvancılık Modülü

**Ahır Yönetimi**
- Ahır listesi, ekleme, düzenleme, silme
- Ahır detay: besi stok durumu + günlük girişler + süt kayıtları + bakım takvimi

**Besi Yönetimi**
- Ahıra bağlı besi stok kaydı (tür, mevcut miktar, eşik miktar)
- Üretici günlük besi girişi yapar → stok otomatik düşer
- Stok eşik altındaysa detay sayfasında kırmızı uyarı badge'i

**Süt Üretimi**
- Günlük süt miktarı kaydı (ahır + mandıra seçimi)
- Mandıra dashboard'unda Chart.js ile haftalık grafik

**Veteriner Bakım Takvimi**
- Veteriner bakım planı oluşturur (tür, tarih)
- Gerçekleşme tarihi ve notları sonradan eklenir
- Tamamlandı işaretleme

**Hayvan Hastalık Bildirimi**
- Üretici ahır seçerek açıklama girer → veterinere atanır
- Veteriner durumu günceller (İnceleniyor / Çözüldü)

---

## 8. Haftalık Geliştirme Planı

### Hafta 1 — Altyapı ve Kimlik Doğrulama (~25 saat)

| Gün | Görev | Süre |
|---|---|---|
| 1 | Solution kurulumu, proje referansları, NuGet paketleri | 5 saat |
| 2 | Core katmanı: tüm entity sınıfları ve enum'lar | 5 saat |
| 3 | Data katmanı: AppDbContext, Identity yapılandırması, ilk migration | 5 saat |
| 4 | Identity: rol tanımları, seed verisi, Login/Register sayfaları | 5 saat |
| 5 | Web: _Layout.cshtml (navbar + sidebar), role göre dinamik menü, dashboard iskeleti | 5 saat |

**Çıktı:** Login olunabilen, role göre farklı menü gösteren çalışır iskelet.

---

### Hafta 2 — Tarım Modülü: Arazi & Ekin (~25 saat)

| Gün | Görev | Süre |
|---|---|---|
| 1 | Arazi Controller + Repository + Service | 5 saat |
| 2 | Arazi Views: Liste, Detay, Ekle/Düzenle | 5 saat |
| 3 | Leaflet.js entegrasyonu (arazi detay sayfası) | 5 saat |
| 4 | Ekin Controller + Service + Views | 5 saat |
| 5 | Tarım işlem (sulama/ilaçlama/toplama) CRUD | 5 saat |

**Çıktı:** Arazi haritada görünür, ekine işlem kaydedilebilir.

---

### Hafta 3 — Tarım Modülü: Temin, Hastalık & Hava Durumu (~25 saat)

| Gün | Görev | Süre |
|---|---|---|
| 1 | OpenWeatherMap API servisi | 5 saat |
| 2 | Arazi detayına hava durumu kartı entegrasyonu | 5 saat |
| 3 | Ürün Temin yönetimi CRUD | 5 saat |
| 4 | Tarım hastalık bildirimi: üretici oluşturur, mühendise atanır | 5 saat |
| 5 | Ziraat mühendisi ekranı: bildirim listesi, durum güncelleme | 5 saat |

**Çıktı:** Hava durumu görünür, hastalık bildirimi akışı tamamdır.

---

### Hafta 4 — Hayvancılık Modülü (~25 saat)

| Gün | Görev | Süre |
|---|---|---|
| 1 | Ahır Controller + Service + Views | 5 saat |
| 2 | Besi Stok yönetimi + eşik göstergesi | 5 saat |
| 3 | Günlük Besi Girişi + stok otomatik güncelleme | 5 saat |
| 4 | Süt Üretimi kaydı + Chart.js grafiği | 5 saat |
| 5 | Hayvan hastalık bildirimi akışı | 5 saat |

**Çıktı:** Ahır yönetimi, besi stok takibi ve süt kaydı çalışır.

---

### Hafta 5 — Veteriner Takvimi & Dashboard'lar (~25 saat)

| Gün | Görev | Süre |
|---|---|---|
| 1 | Veteriner bakım takvimi CRUD + veteriner ekranı | 5 saat |
| 2 | Hayvan hastalık bildiriminde veteriner akışı | 5 saat |
| 3 | Üretici dashboard: özet kartlar + yaklaşan işlemler | 5 saat |
| 4 | Diğer rol dashboard'ları (Yönetici, Veteriner, Mandıra, Toptancı, Tedarikçi) | 5 saat |
| 5 | Rol bazlı yetki kontrollerinin gözden geçirilmesi | 5 saat |

**Çıktı:** Tüm modüller ve dashboard'lar tamamdır.

---

### Hafta 6 — Test, Düzeltme ve Sunum (~15 saat)

| Gün | Görev | Süre |
|---|---|---|
| 1 | Uçtan uca senaryo testi (her rol ile manuel test) | 5 saat |
| 2 | Hata düzeltme, eksik validasyon tamamlama | 5 saat |
| 3 | UI/UX iyileştirme, responsive kontrol, demo hazırlığı | 5 saat |

**Çıktı:** Sunuma hazır, stabil çalışan uygulama.

---

## 9. Kapsam Dışı (İlk Aşama)

| Özellik | Neden Ertelendi |
|---|---|
| SignalR gerçek zamanlı bildirimler | Ekstra altyapı ve öğrenme süresi |
| Hangfire zamanlanmış görevler | Arka plan servis yönetimi gerektirir |
| LLM entegrasyonu (hava bazlı öneri mesajları) | API maliyeti + entegrasyon karmaşıklığı |
| E-posta / SMS bildirimi | Dış servis bağımlılığı |
| Otomatik stok tedarik bildirimi | Hangfire'a bağımlı |
| PDF export ve gelişmiş raporlama | Kapsam genişlemesi |

---

## 10. Geliştirme Ortamı Kurulumu

### Gereksinimler
- .NET 8 SDK
- Visual Studio 2022 veya VS Code + C# Dev Kit
- SQL Server Express / LocalDB
- OpenWeatherMap API anahtarı (ücretsiz kayıt: openweathermap.org)

### Proje Kurulum Komutları

```bash
# Solution ve projeler
dotnet new sln -n KooperatifYonetim
dotnet new classlib -n KooperatifYonetim.Core
dotnet new classlib -n KooperatifYonetim.Data
dotnet new classlib -n KooperatifYonetim.Business
dotnet new mvc -n KooperatifYonetim.Web

# Solution'a ekle
dotnet sln add KooperatifYonetim.Core
dotnet sln add KooperatifYonetim.Data
dotnet sln add KooperatifYonetim.Business
dotnet sln add KooperatifYonetim.Web

# Proje referansları
dotnet add KooperatifYonetim.Data reference KooperatifYonetim.Core
dotnet add KooperatifYonetim.Business reference KooperatifYonetim.Core
dotnet add KooperatifYonetim.Business reference KooperatifYonetim.Data
dotnet add KooperatifYonetim.Web reference KooperatifYonetim.Business

# NuGet paketleri — Data
dotnet add KooperatifYonetim.Data package Microsoft.EntityFrameworkCore.SqlServer
dotnet add KooperatifYonetim.Data package Microsoft.EntityFrameworkCore.Tools
dotnet add KooperatifYonetim.Data package Microsoft.AspNetCore.Identity.EntityFrameworkCore

# NuGet paketleri — Business
dotnet add KooperatifYonetim.Business package AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet add KooperatifYonetim.Business package Newtonsoft.Json

# İlk migration
dotnet ef migrations add InitialCreate --project KooperatifYonetim.Data --startup-project KooperatifYonetim.Web
dotnet ef database update --project KooperatifYonetim.Data --startup-project KooperatifYonetim.Web
```

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=KooperatifYonetimDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "OpenWeatherMap": {
    "ApiKey": "YOUR_API_KEY_HERE",
    "BaseUrl": "https://api.openweathermap.org/data/2.5"
  }
}
```

### Seed Kullanıcıları (Geliştirme için)

| E-posta | Şifre | Rol |
|---|---|---|
| yonetici@test.com | Sifre123! | Yonetici |
| uretici@test.com | Sifre123! | Uretici |
| veteriner@test.com | Sifre123! | Veteriner |
| muhendis@test.com | Sifre123! | ZiraatMuhendisi |
| toptanci@test.com | Sifre123! | Toptanci |
| mandira@test.com | Sifre123! | Mandira |
| tedarikci@test.com | Sifre123! | Tedarikci |

---

> **Claude Code Kullanım Notu:** Bu dosyayı projenin kök dizinine `PLAN.md` olarak kaydedin. Claude Code oturumunu başlatırken "PLAN.md dosyasını referans alarak Hafta 1'den başla, adım adım uygula" komutunu verin. Her haftanın sonunda bir sonraki haftanın detaylarını bu dosyadan referans alarak devam edin.
