using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KooperatifYonetim.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixShadowFKs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
