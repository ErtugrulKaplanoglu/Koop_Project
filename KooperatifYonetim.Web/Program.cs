using KooperatifYonetim.Business.Services;
using KooperatifYonetim.Core.Entities;
using KooperatifYonetim.Core.Interfaces;
using KooperatifYonetim.Data;
using KooperatifYonetim.Data.Seeds;
using KooperatifYonetim.Web.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = true;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/Login";
});

builder.Services.AddScoped<IAraziService, AraziService>();
builder.Services.AddScoped<IEkinService, EkinService>();
builder.Services.AddScoped<IEkinTuruService, EkinTuruService>();
builder.Services.AddScoped<ITarimIslemService, TarimIslemService>();
builder.Services.AddScoped<IUrunTeminService, UrunTeminService>();
builder.Services.AddScoped<ITarimHastalikService, TarimHastalikService>();

builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IAhirService, AhirService>();
builder.Services.AddScoped<IBesiStokService, BesiStokService>();
builder.Services.AddScoped<IBildirimService, BildirimService>();
builder.Services.AddScoped<IYemTedarikService, YemTedarikService>();
builder.Services.AddScoped<ISutUretimiService, SutUretimiService>();
builder.Services.AddScoped<IVeterinerBakimService, VeterinerBakimService>();
builder.Services.AddScoped<IHayvanHastalikService, HayvanHastalikService>();
builder.Services.AddScoped<IYemTuruService, YemTuruService>();
builder.Services.AddScoped<IUrunFiyatService, UrunFiyatService>();
builder.Services.AddScoped<IOdemeDonemiService, OdemeDonemiService>();
builder.Services.AddScoped<IRaporService, RaporService>();
builder.Services.AddScoped<IMesajService, MesajService>();

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<IWeatherService, WeatherService>();

builder.Services.AddControllersWithViews(options =>
    options.ModelBinderProviders.Insert(0, new InvariantDoubleModelBinderProvider()));

var app = builder.Build();

// Seed veritabanı
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await db.Database.MigrateAsync();
    await SeedData.SeedRolesAsync(roleManager);
    await SeedData.SeedUsersAsync(userManager);
    await SeedData.SeedEkinTurleriAsync(db);
    await SeedData.SeedYemTurleriAsync(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();

app.Run();
