using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KooperatifYonetim.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Soyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ahirlar",
                columns: table => new
                {
                    AhirId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HayvanSayisi = table.Column<int>(type: "int", nullable: false),
                    UreticiId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ahirlar", x => x.AhirId);
                    table.ForeignKey(
                        name: "FK_Ahirlar_AspNetUsers_UreticiId",
                        column: x => x.UreticiId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Araziler",
                columns: table => new
                {
                    AraziId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enlem = table.Column<double>(type: "float", nullable: false),
                    Boylam = table.Column<double>(type: "float", nullable: false),
                    YuzOlcumu = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    UreticiId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Araziler", x => x.AraziId);
                    table.ForeignKey(
                        name: "FK_Araziler_AspNetUsers_UreticiId",
                        column: x => x.UreticiId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BesiStoklar",
                columns: table => new
                {
                    StokId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AhirId = table.Column<int>(type: "int", nullable: false),
                    BesiTuru = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MevcutMiktar = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    EsikMiktar = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    SonGuncelleme = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BesiStoklar", x => x.StokId);
                    table.ForeignKey(
                        name: "FK_BesiStoklar_Ahirlar_AhirId",
                        column: x => x.AhirId,
                        principalTable: "Ahirlar",
                        principalColumn: "AhirId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GunlukBesiGirisleri",
                columns: table => new
                {
                    GirisId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AhirId = table.Column<int>(type: "int", nullable: false),
                    BesiTuru = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YedirildenMiktar = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GunlukBesiGirisleri", x => x.GirisId);
                    table.ForeignKey(
                        name: "FK_GunlukBesiGirisleri_Ahirlar_AhirId",
                        column: x => x.AhirId,
                        principalTable: "Ahirlar",
                        principalColumn: "AhirId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HayvanHastalikBildirimler",
                columns: table => new
                {
                    BildirimId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AhirId = table.Column<int>(type: "int", nullable: false),
                    UreticiId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VeterinerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BildirimTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HayvanHastalikBildirimler", x => x.BildirimId);
                    table.ForeignKey(
                        name: "FK_HayvanHastalikBildirimler_Ahirlar_AhirId",
                        column: x => x.AhirId,
                        principalTable: "Ahirlar",
                        principalColumn: "AhirId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HayvanHastalikBildirimler_AspNetUsers_UreticiId",
                        column: x => x.UreticiId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HayvanHastalikBildirimler_AspNetUsers_VeterinerId",
                        column: x => x.VeterinerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SutUretimleri",
                columns: table => new
                {
                    SutId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AhirId = table.Column<int>(type: "int", nullable: false),
                    MandiraId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Miktar = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SutUretimleri", x => x.SutId);
                    table.ForeignKey(
                        name: "FK_SutUretimleri_Ahirlar_AhirId",
                        column: x => x.AhirId,
                        principalTable: "Ahirlar",
                        principalColumn: "AhirId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SutUretimleri_AspNetUsers_MandiraId",
                        column: x => x.MandiraId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VeterinerBakimlar",
                columns: table => new
                {
                    BakimId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AhirId = table.Column<int>(type: "int", nullable: false),
                    VeterinerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlanlananTarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GerceklesenTarih = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BakimTuru = table.Column<int>(type: "int", nullable: false),
                    Notlar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tamamlandi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VeterinerBakimlar", x => x.BakimId);
                    table.ForeignKey(
                        name: "FK_VeterinerBakimlar_Ahirlar_AhirId",
                        column: x => x.AhirId,
                        principalTable: "Ahirlar",
                        principalColumn: "AhirId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VeterinerBakimlar_AspNetUsers_VeterinerId",
                        column: x => x.VeterinerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ekinler",
                columns: table => new
                {
                    EkinId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AraziId = table.Column<int>(type: "int", nullable: false),
                    EkinTuru = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EkimTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasatTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Durum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ekinler", x => x.EkinId);
                    table.ForeignKey(
                        name: "FK_Ekinler_Araziler_AraziId",
                        column: x => x.AraziId,
                        principalTable: "Araziler",
                        principalColumn: "AraziId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UrunTeminler",
                columns: table => new
                {
                    UrunTeminId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AraziId = table.Column<int>(type: "int", nullable: false),
                    ToptanciId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Donem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlanlananMiktar = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    AlinanMiktar = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunTeminler", x => x.UrunTeminId);
                    table.ForeignKey(
                        name: "FK_UrunTeminler_Araziler_AraziId",
                        column: x => x.AraziId,
                        principalTable: "Araziler",
                        principalColumn: "AraziId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UrunTeminler_AspNetUsers_ToptanciId",
                        column: x => x.ToptanciId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TarimHastalikBildirimler",
                columns: table => new
                {
                    BildirimId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EkinId = table.Column<int>(type: "int", nullable: false),
                    UreticiId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MuhendisId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BildirimTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TarimHastalikBildirimler", x => x.BildirimId);
                    table.ForeignKey(
                        name: "FK_TarimHastalikBildirimler_AspNetUsers_MuhendisId",
                        column: x => x.MuhendisId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TarimHastalikBildirimler_AspNetUsers_UreticiId",
                        column: x => x.UreticiId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TarimHastalikBildirimler_Ekinler_EkinId",
                        column: x => x.EkinId,
                        principalTable: "Ekinler",
                        principalColumn: "EkinId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TarimIslemler",
                columns: table => new
                {
                    IslemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EkinId = table.Column<int>(type: "int", nullable: false),
                    IslemTuru = table.Column<int>(type: "int", nullable: false),
                    PlanlananTarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GerceklesenTarih = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Miktar = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    Notlar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tamamlandi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TarimIslemler", x => x.IslemId);
                    table.ForeignKey(
                        name: "FK_TarimIslemler_Ekinler_EkinId",
                        column: x => x.EkinId,
                        principalTable: "Ekinler",
                        principalColumn: "EkinId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ahirlar_UreticiId",
                table: "Ahirlar",
                column: "UreticiId");

            migrationBuilder.CreateIndex(
                name: "IX_Araziler_UreticiId",
                table: "Araziler",
                column: "UreticiId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BesiStoklar_AhirId",
                table: "BesiStoklar",
                column: "AhirId");

            migrationBuilder.CreateIndex(
                name: "IX_Ekinler_AraziId",
                table: "Ekinler",
                column: "AraziId");

            migrationBuilder.CreateIndex(
                name: "IX_GunlukBesiGirisleri_AhirId",
                table: "GunlukBesiGirisleri",
                column: "AhirId");

            migrationBuilder.CreateIndex(
                name: "IX_HayvanHastalikBildirimler_AhirId",
                table: "HayvanHastalikBildirimler",
                column: "AhirId");

            migrationBuilder.CreateIndex(
                name: "IX_HayvanHastalikBildirimler_UreticiId",
                table: "HayvanHastalikBildirimler",
                column: "UreticiId");

            migrationBuilder.CreateIndex(
                name: "IX_HayvanHastalikBildirimler_VeterinerId",
                table: "HayvanHastalikBildirimler",
                column: "VeterinerId");

            migrationBuilder.CreateIndex(
                name: "IX_SutUretimleri_AhirId",
                table: "SutUretimleri",
                column: "AhirId");

            migrationBuilder.CreateIndex(
                name: "IX_SutUretimleri_MandiraId",
                table: "SutUretimleri",
                column: "MandiraId");

            migrationBuilder.CreateIndex(
                name: "IX_TarimHastalikBildirimler_EkinId",
                table: "TarimHastalikBildirimler",
                column: "EkinId");

            migrationBuilder.CreateIndex(
                name: "IX_TarimHastalikBildirimler_MuhendisId",
                table: "TarimHastalikBildirimler",
                column: "MuhendisId");

            migrationBuilder.CreateIndex(
                name: "IX_TarimHastalikBildirimler_UreticiId",
                table: "TarimHastalikBildirimler",
                column: "UreticiId");

            migrationBuilder.CreateIndex(
                name: "IX_TarimIslemler_EkinId",
                table: "TarimIslemler",
                column: "EkinId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunTeminler_AraziId",
                table: "UrunTeminler",
                column: "AraziId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunTeminler_ToptanciId",
                table: "UrunTeminler",
                column: "ToptanciId");

            migrationBuilder.CreateIndex(
                name: "IX_VeterinerBakimlar_AhirId",
                table: "VeterinerBakimlar",
                column: "AhirId");

            migrationBuilder.CreateIndex(
                name: "IX_VeterinerBakimlar_VeterinerId",
                table: "VeterinerBakimlar",
                column: "VeterinerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BesiStoklar");

            migrationBuilder.DropTable(
                name: "GunlukBesiGirisleri");

            migrationBuilder.DropTable(
                name: "HayvanHastalikBildirimler");

            migrationBuilder.DropTable(
                name: "SutUretimleri");

            migrationBuilder.DropTable(
                name: "TarimHastalikBildirimler");

            migrationBuilder.DropTable(
                name: "TarimIslemler");

            migrationBuilder.DropTable(
                name: "UrunTeminler");

            migrationBuilder.DropTable(
                name: "VeterinerBakimlar");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Ekinler");

            migrationBuilder.DropTable(
                name: "Ahirlar");

            migrationBuilder.DropTable(
                name: "Araziler");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
