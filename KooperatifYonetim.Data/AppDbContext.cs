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
        public DbSet<TarimIslem> TarimIslemler { get; set; }
        public DbSet<UrunTemin> UrunTeminler { get; set; }
        public DbSet<TarimHastalikBildirimi> TarimHastalikBildirimler { get; set; }
        public DbSet<Ahir> Ahirlar { get; set; }
        public DbSet<BesiStok> BesiStoklar { get; set; }
        public DbSet<GunlukBesiGirisi> GunlukBesiGirisleri { get; set; }
        public DbSet<SutUretimi> SutUretimleri { get; set; }
        public DbSet<VeterinerBakim> VeterinerBakimlar { get; set; }
        public DbSet<HayvanHastalikBildirimi> HayvanHastalikBildirimler { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Arazi>().Property(a => a.YuzOlcumu).HasPrecision(10, 2);
            builder.Entity<BesiStok>().Property(b => b.MevcutMiktar).HasPrecision(10, 2);
            builder.Entity<BesiStok>().Property(b => b.EsikMiktar).HasPrecision(10, 2);
            builder.Entity<GunlukBesiGirisi>().Property(g => g.YedirildenMiktar).HasPrecision(10, 2);
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
        }
    }
}
