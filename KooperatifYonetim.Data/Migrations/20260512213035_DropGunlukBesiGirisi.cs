using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KooperatifYonetim.Data.Migrations
{
    /// <inheritdoc />
    public partial class DropGunlukBesiGirisi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GunlukBesiGirisi");

            migrationBuilder.AddColumn<int>(
                name: "AhirId1",
                table: "YemTedarikBasvurulari",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AhirId1",
                table: "BesiHareketleri",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_YemTedarikBasvurulari_AhirId1",
                table: "YemTedarikBasvurulari",
                column: "AhirId1");

            migrationBuilder.CreateIndex(
                name: "IX_BesiHareketleri_AhirId1",
                table: "BesiHareketleri",
                column: "AhirId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BesiHareketleri_Ahirlar_AhirId1",
                table: "BesiHareketleri",
                column: "AhirId1",
                principalTable: "Ahirlar",
                principalColumn: "AhirId");

            migrationBuilder.AddForeignKey(
                name: "FK_YemTedarikBasvurulari_Ahirlar_AhirId1",
                table: "YemTedarikBasvurulari",
                column: "AhirId1",
                principalTable: "Ahirlar",
                principalColumn: "AhirId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BesiHareketleri_Ahirlar_AhirId1",
                table: "BesiHareketleri");

            migrationBuilder.DropForeignKey(
                name: "FK_YemTedarikBasvurulari_Ahirlar_AhirId1",
                table: "YemTedarikBasvurulari");

            migrationBuilder.DropIndex(
                name: "IX_YemTedarikBasvurulari_AhirId1",
                table: "YemTedarikBasvurulari");

            migrationBuilder.DropIndex(
                name: "IX_BesiHareketleri_AhirId1",
                table: "BesiHareketleri");

            migrationBuilder.DropColumn(
                name: "AhirId1",
                table: "YemTedarikBasvurulari");

            migrationBuilder.DropColumn(
                name: "AhirId1",
                table: "BesiHareketleri");

            migrationBuilder.CreateTable(
                name: "GunlukBesiGirisi",
                columns: table => new
                {
                    GirisId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AhirId = table.Column<int>(type: "int", nullable: false),
                    YemTuruId = table.Column<int>(type: "int", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    YedirildenMiktar = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GunlukBesiGirisi", x => x.GirisId);
                    table.ForeignKey(
                        name: "FK_GunlukBesiGirisi_Ahirlar_AhirId",
                        column: x => x.AhirId,
                        principalTable: "Ahirlar",
                        principalColumn: "AhirId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GunlukBesiGirisi_YemTurleri_YemTuruId",
                        column: x => x.YemTuruId,
                        principalTable: "YemTurleri",
                        principalColumn: "YemTuruId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GunlukBesiGirisi_AhirId",
                table: "GunlukBesiGirisi",
                column: "AhirId");

            migrationBuilder.CreateIndex(
                name: "IX_GunlukBesiGirisi_YemTuruId",
                table: "GunlukBesiGirisi",
                column: "YemTuruId");
        }
    }
}
