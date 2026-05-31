using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KooperatifYonetim.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBildirimVeHareketSistemi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GunlukBesiGirisleri_Ahirlar_AhirId",
                table: "GunlukBesiGirisleri");

            migrationBuilder.DropForeignKey(
                name: "FK_GunlukBesiGirisleri_YemTurleri_YemTuruId",
                table: "GunlukBesiGirisleri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GunlukBesiGirisleri",
                table: "GunlukBesiGirisleri");

            migrationBuilder.RenameTable(
                name: "GunlukBesiGirisleri",
                newName: "GunlukBesiGirisi");

            migrationBuilder.RenameIndex(
                name: "IX_GunlukBesiGirisleri_YemTuruId",
                table: "GunlukBesiGirisi",
                newName: "IX_GunlukBesiGirisi_YemTuruId");

            migrationBuilder.RenameIndex(
                name: "IX_GunlukBesiGirisleri_AhirId",
                table: "GunlukBesiGirisi",
                newName: "IX_GunlukBesiGirisi_AhirId");

            migrationBuilder.AlterColumn<decimal>(
                name: "YedirildenMiktar",
                table: "GunlukBesiGirisi",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GunlukBesiGirisi",
                table: "GunlukBesiGirisi",
                column: "GirisId");

            migrationBuilder.CreateTable(
                name: "BesiHareketleri",
                columns: table => new
                {
                    HareketId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AhirId = table.Column<int>(type: "int", nullable: false),
                    YemTuruId = table.Column<int>(type: "int", nullable: false),
                    HareketTipi = table.Column<int>(type: "int", nullable: false),
                    Miktar = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notlar = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BesiHareketleri", x => x.HareketId);
                    table.ForeignKey(
                        name: "FK_BesiHareketleri_Ahirlar_AhirId",
                        column: x => x.AhirId,
                        principalTable: "Ahirlar",
                        principalColumn: "AhirId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BesiHareketleri_YemTurleri_YemTuruId",
                        column: x => x.YemTuruId,
                        principalTable: "YemTurleri",
                        principalColumn: "YemTuruId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bildirimler",
                columns: table => new
                {
                    BildirimId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AliciId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Baslik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mesaj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BildirimTipi = table.Column<int>(type: "int", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Okundu = table.Column<bool>(type: "bit", nullable: false),
                    IlgiliKayitId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bildirimler", x => x.BildirimId);
                    table.ForeignKey(
                        name: "FK_Bildirimler_AspNetUsers_AliciId",
                        column: x => x.AliciId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YemTedarikBasvurulari",
                columns: table => new
                {
                    BasvuruId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AhirId = table.Column<int>(type: "int", nullable: false),
                    UreticiId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    YemTuruId = table.Column<int>(type: "int", nullable: false),
                    TalepMiktar = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BasvuruTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    YoneticiNotu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YemTedarikBasvurulari", x => x.BasvuruId);
                    table.ForeignKey(
                        name: "FK_YemTedarikBasvurulari_Ahirlar_AhirId",
                        column: x => x.AhirId,
                        principalTable: "Ahirlar",
                        principalColumn: "AhirId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_YemTedarikBasvurulari_AspNetUsers_UreticiId",
                        column: x => x.UreticiId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_YemTedarikBasvurulari_YemTurleri_YemTuruId",
                        column: x => x.YemTuruId,
                        principalTable: "YemTurleri",
                        principalColumn: "YemTuruId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BesiHareketleri_AhirId",
                table: "BesiHareketleri",
                column: "AhirId");

            migrationBuilder.CreateIndex(
                name: "IX_BesiHareketleri_YemTuruId",
                table: "BesiHareketleri",
                column: "YemTuruId");

            migrationBuilder.CreateIndex(
                name: "IX_Bildirimler_AliciId",
                table: "Bildirimler",
                column: "AliciId");

            migrationBuilder.CreateIndex(
                name: "IX_YemTedarikBasvurulari_AhirId",
                table: "YemTedarikBasvurulari",
                column: "AhirId");

            migrationBuilder.CreateIndex(
                name: "IX_YemTedarikBasvurulari_UreticiId",
                table: "YemTedarikBasvurulari",
                column: "UreticiId");

            migrationBuilder.CreateIndex(
                name: "IX_YemTedarikBasvurulari_YemTuruId",
                table: "YemTedarikBasvurulari",
                column: "YemTuruId");

            migrationBuilder.AddForeignKey(
                name: "FK_GunlukBesiGirisi_Ahirlar_AhirId",
                table: "GunlukBesiGirisi",
                column: "AhirId",
                principalTable: "Ahirlar",
                principalColumn: "AhirId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GunlukBesiGirisi_YemTurleri_YemTuruId",
                table: "GunlukBesiGirisi",
                column: "YemTuruId",
                principalTable: "YemTurleri",
                principalColumn: "YemTuruId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GunlukBesiGirisi_Ahirlar_AhirId",
                table: "GunlukBesiGirisi");

            migrationBuilder.DropForeignKey(
                name: "FK_GunlukBesiGirisi_YemTurleri_YemTuruId",
                table: "GunlukBesiGirisi");

            migrationBuilder.DropTable(
                name: "BesiHareketleri");

            migrationBuilder.DropTable(
                name: "Bildirimler");

            migrationBuilder.DropTable(
                name: "YemTedarikBasvurulari");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GunlukBesiGirisi",
                table: "GunlukBesiGirisi");

            migrationBuilder.RenameTable(
                name: "GunlukBesiGirisi",
                newName: "GunlukBesiGirisleri");

            migrationBuilder.RenameIndex(
                name: "IX_GunlukBesiGirisi_YemTuruId",
                table: "GunlukBesiGirisleri",
                newName: "IX_GunlukBesiGirisleri_YemTuruId");

            migrationBuilder.RenameIndex(
                name: "IX_GunlukBesiGirisi_AhirId",
                table: "GunlukBesiGirisleri",
                newName: "IX_GunlukBesiGirisleri_AhirId");

            migrationBuilder.AlterColumn<decimal>(
                name: "YedirildenMiktar",
                table: "GunlukBesiGirisleri",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GunlukBesiGirisleri",
                table: "GunlukBesiGirisleri",
                column: "GirisId");

            migrationBuilder.AddForeignKey(
                name: "FK_GunlukBesiGirisleri_Ahirlar_AhirId",
                table: "GunlukBesiGirisleri",
                column: "AhirId",
                principalTable: "Ahirlar",
                principalColumn: "AhirId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GunlukBesiGirisleri_YemTurleri_YemTuruId",
                table: "GunlukBesiGirisleri",
                column: "YemTuruId",
                principalTable: "YemTurleri",
                principalColumn: "YemTuruId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
