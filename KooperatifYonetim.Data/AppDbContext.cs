using KooperatifYonetim.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KooperatifYonetim.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Arazi> Araziler { get; set; }
        public DbSet<Ekin> Ekinler { get; set; }
        public DbSet<EkinTuru> EkinTurleri { get; set; }
        public DbSet<TarimIslem> TarimIslemler { get; set; }
        public DbSet<UrunTemin> UrunTeminler { get; set; }
        public DbSet<TarimHastalikBildirimi> TarimHastalikBildirimler { get; set; }
        public DbSet<Ahir> Ahirlar { get; set; }
        public DbSet<BesiStok> BesiStoklar { get; set; }
        public DbSet<BesiHareketi> BesiHareketleri { get; set; }
        public DbSet<Bildirim> Bildirimler { get; set; }
        public DbSet<YemTedarikBasvuru> YemTedarikBasvurulari { get; set; }
        public DbSet<SutUretimi> SutUretimleri { get; set; }
        public DbSet<VeterinerBakim> VeterinerBakimlar { get; set; }
        public DbSet<HayvanHastalikBildirimi> HayvanHastalikBildirimler { get; set; }
        public DbSet<YemTuru> YemTurleri { get; set; }
        public DbSet<UrunFiyat> UrunFiyatlar { get; set; }
        public DbSet<OdemeDonemi> OdemeDonemleri { get; set; }
        public DbSet<UreticiOdeme> UreticiOdemeler { get; set; }
        public DbSet<Mesaj> Mesajlar { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Arazi>().Property(a => a.YuzOlcumu).HasPrecision(10, 2);
            builder.Entity<BesiStok>().Property(b => b.MevcutMiktar).HasPrecision(10, 2);
            builder.Entity<BesiStok>().Property(b => b.EsikMiktar).HasPrecision(10, 2);
            builder.Entity<BesiHareketi>().Property(b => b.Miktar).HasPrecision(10, 2);
            builder.Entity<YemTedarikBasvuru>().Property(y => y.TalepMiktar).HasPrecision(10, 2);
            builder.Entity<SutUretimi>().Property(s => s.Miktar).HasPrecision(10, 2);
            builder.Entity<TarimIslem>().Property(t => t.Miktar).HasPrecision(10, 2);
            builder.Entity<UrunTemin>().Property(u => u.PlanlananMiktar).HasPrecision(10, 2);
            builder.Entity<UrunTemin>().Property(u => u.AlinanMiktar).HasPrecision(10, 2);

            builder.Entity<Arazi>()
                .HasOne(a => a.Uretici)
                .WithMany(u => u.Araziler)
                .HasForeignKey(a => a.UreticiId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Ahir>()
                .HasOne(a => a.Uretici)
                .WithMany(u => u.Ahirlar)
                .HasForeignKey(a => a.UreticiId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UrunTemin>()
                .HasOne(u => u.Toptanci)
                .WithMany()
                .HasForeignKey(u => u.ToptanciId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TarimHastalikBildirimi>()
                .HasOne(t => t.Uretici)
                .WithMany()
                .HasForeignKey(t => t.UreticiId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TarimHastalikBildirimi>()
                .HasOne(t => t.Muhendis)
                .WithMany()
                .HasForeignKey(t => t.MuhendisId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SutUretimi>()
                .HasOne(s => s.Mandira)
                .WithMany()
                .HasForeignKey(s => s.MandiraId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<VeterinerBakim>()
                .HasOne(v => v.Veteriner)
                .WithMany()
                .HasForeignKey(v => v.VeterinerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<HayvanHastalikBildirimi>()
                .HasOne(h => h.Uretici)
                .WithMany()
                .HasForeignKey(h => h.UreticiId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<HayvanHastalikBildirimi>()
                .HasOne(h => h.Veteriner)
                .WithMany()
                .HasForeignKey(h => h.VeterinerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Ekin>()
                .HasOne(e => e.EkinTuruNavigation)
                .WithMany(et => et.Ekinler)
                .HasForeignKey(e => e.EkinTuruId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BesiStok>()
                .HasOne(b => b.YemTuru)
                .WithMany(yt => yt.BesiStoklar)
                .HasForeignKey(b => b.YemTuruId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BesiHareketi>()
                .HasOne(b => b.Ahir)
                .WithMany(a => a.BesiHareketleri)
                .HasForeignKey(b => b.AhirId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BesiHareketi>()
                .HasOne(b => b.YemTuru)
                .WithMany(yt => yt.BesiHareketleri)
                .HasForeignKey(b => b.YemTuruId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Bildirim>()
                .HasOne(b => b.Alici)
                .WithMany()
                .HasForeignKey(b => b.AliciId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<YemTedarikBasvuru>()
                .HasOne(y => y.Ahir)
                .WithMany(a => a.YemTedarikBasvurular)
                .HasForeignKey(y => y.AhirId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<YemTedarikBasvuru>()
                .HasOne(y => y.Uretici)
                .WithMany()
                .HasForeignKey(y => y.UreticiId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<YemTedarikBasvuru>()
                .HasOne(y => y.YemTuru)
                .WithMany(yt => yt.TedarikBasvurulari)
                .HasForeignKey(y => y.YemTuruId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UrunFiyat>()
                .Property(u => u.BirimFiyat).HasPrecision(10, 2);

            builder.Entity<UrunFiyat>()
                .HasOne(u => u.EkinTuru)
                .WithMany()
                .HasForeignKey(u => u.EkinTuruId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UreticiOdeme>()
                .Property(u => u.ToplamEkinKg).HasPrecision(10, 2);
            builder.Entity<UreticiOdeme>()
                .Property(u => u.ToplamSutLitre).HasPrecision(10, 2);
            builder.Entity<UreticiOdeme>()
                .Property(u => u.ToplamTutar).HasPrecision(12, 2);

            builder.Entity<UreticiOdeme>()
                .HasOne(u => u.OdemeDonemi)
                .WithMany(od => od.UreticiOdemeler)
                .HasForeignKey(u => u.OdemeDonemiId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UreticiOdeme>()
                .HasOne(u => u.Uretici)
                .WithMany()
                .HasForeignKey(u => u.UreticiId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OdemeDonemi>()
                .HasIndex(od => new { od.Yil, od.Ay })
                .IsUnique();

            builder.Entity<Mesaj>()
                .HasOne(m => m.Gonderen)
                .WithMany()
                .HasForeignKey(m => m.GonderenId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Mesaj>()
                .HasOne(m => m.Alici)
                .WithMany()
                .HasForeignKey(m => m.AliciId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
