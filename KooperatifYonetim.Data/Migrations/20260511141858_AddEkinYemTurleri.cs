using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KooperatifYonetim.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEkinYemTurleri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BesiTuru",
                table: "GunlukBesiGirisleri");

            migrationBuilder.DropColumn(
                name: "EkinTuru",
                table: "Ekinler");

            migrationBuilder.DropColumn(
                name: "BesiTuru",
                table: "BesiStoklar");

            migrationBuilder.AddColumn<int>(
                name: "YemTuruId",
                table: "GunlukBesiGirisleri",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EkinTuruId",
                table: "Ekinler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YemTuruId",
                table: "BesiStoklar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EkinTurleri",
                columns: table => new
                {
                    EkinTuruId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToplamaTipi = table.Column<int>(type: "int", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EkinTurleri", x => x.EkinTuruId);
                });

            migrationBuilder.CreateTable(
                name: "OdemeDonemleri",
                columns: table => new
                {
                    OdemeDonemiId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Yil = table.Column<int>(type: "int", nullable: false),
                    Ay = table.Column<int>(type: "int", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    OnayTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdemeDonemleri", x => x.OdemeDonemiId);
                });

            migrationBuilder.CreateTable(
                name: "YemTurleri",
                columns: table => new
                {
                    YemTuruId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birim = table.Column<int>(type: "int", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YemTurleri", x => x.YemTuruId);
                });

            migrationBuilder.CreateTable(
                name: "UrunFiyatlar",
                columns: table => new
                {
                    UrunFiyatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EkinTuruId = table.Column<int>(type: "int", nullable: true),
                    SutMu = table.Column<bool>(type: "bit", nullable: false),
                    BirimFiyat = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    BaslangicTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunFiyatlar", x => x.UrunFiyatId);
                    table.ForeignKey(
                        name: "FK_UrunFiyatlar_EkinTurleri_EkinTuruId",
                        column: x => x.EkinTuruId,
                        principalTable: "EkinTurleri",
                        principalColumn: "EkinTuruId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UreticiOdemeler",
                columns: table => new
                {
                    UreticiOdemeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OdemeDonemiId = table.Column<int>(type: "int", nullable: false),
                    UreticiId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ToplamEkinKg = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ToplamSutLitre = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ToplamTutar = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    OdemeDetay = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UreticiOdemeler", x => x.UreticiOdemeId);
                    table.ForeignKey(
                        name: "FK_UreticiOdemeler_AspNetUsers_UreticiId",
                        column: x => x.UreticiId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UreticiOdemeler_OdemeDonemleri_OdemeDonemiId",
                        column: x => x.OdemeDonemiId,
                        principalTable: "OdemeDonemleri",
                        principalColumn: "OdemeDonemiId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GunlukBesiGirisleri_YemTuruId",
                table: "GunlukBesiGirisleri",
                column: "YemTuruId");

            migrationBuilder.CreateIndex(
                name: "IX_Ekinler_EkinTuruId",
                table: "Ekinler",
                column: "EkinTuruId");

            migrationBuilder.CreateIndex(
                name: "IX_BesiStoklar_YemTuruId",
                table: "BesiStoklar",
                column: "YemTuruId");

            migrationBuilder.CreateIndex(
                name: "IX_OdemeDonemleri_Yil_Ay",
                table: "OdemeDonemleri",
                columns: new[] { "Yil", "Ay" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UreticiOdemeler_OdemeDonemiId",
                table: "UreticiOdemeler",
                column: "OdemeDonemiId");

            migrationBuilder.CreateIndex(
                name: "IX_UreticiOdemeler_UreticiId",
                table: "UreticiOdemeler",
                column: "UreticiId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunFiyatlar_EkinTuruId",
                table: "UrunFiyatlar",
                column: "EkinTuruId");

            migrationBuilder.AddForeignKey(
                name: "FK_BesiStoklar_YemTurleri_YemTuruId",
                table: "BesiStoklar",
                column: "YemTuruId",
                principalTable: "YemTurleri",
                principalColumn: "YemTuruId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ekinler_EkinTurleri_EkinTuruId",
                table: "Ekinler",
                column: "EkinTuruId",
                principalTable: "EkinTurleri",
                principalColumn: "EkinTuruId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GunlukBesiGirisleri_YemTurleri_YemTuruId",
                table: "GunlukBesiGirisleri",
                column: "YemTuruId",
                principalTable: "YemTurleri",
                principalColumn: "YemTuruId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BesiStoklar_YemTurleri_YemTuruId",
                table: "BesiStoklar");

            migrationBuilder.DropForeignKey(
                name: "FK_Ekinler_EkinTurleri_EkinTuruId",
                table: "Ekinler");

            migrationBuilder.DropForeignKey(
                name: "FK_GunlukBesiGirisleri_YemTurleri_YemTuruId",
                table: "GunlukBesiGirisleri");

            migrationBuilder.DropTable(
                name: "UreticiOdemeler");

            migrationBuilder.DropTable(
                name: "UrunFiyatlar");

            migrationBuilder.DropTable(
                name: "YemTurleri");

            migrationBuilder.DropTable(
                name: "OdemeDonemleri");

            migrationBuilder.DropTable(
                name: "EkinTurleri");

            migrationBuilder.DropIndex(
                name: "IX_GunlukBesiGirisleri_YemTuruId",
                table: "GunlukBesiGirisleri");

            migrationBuilder.DropIndex(
                name: "IX_Ekinler_EkinTuruId",
                table: "Ekinler");

            migrationBuilder.DropIndex(
                name: "IX_BesiStoklar_YemTuruId",
                table: "BesiStoklar");

            migrationBuilder.DropColumn(
                name: "YemTuruId",
                table: "GunlukBesiGirisleri");

            migrationBuilder.DropColumn(
                name: "EkinTuruId",
                table: "Ekinler");

            migrationBuilder.DropColumn(
                name: "YemTuruId",
                table: "BesiStoklar");

            migrationBuilder.AddColumn<string>(
                name: "BesiTuru",
                table: "GunlukBesiGirisleri",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EkinTuru",
                table: "Ekinler",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BesiTuru",
                table: "BesiStoklar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
