using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HostMaster.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddServicieAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceAvailabilities_ExtraServices_ExtraServiceId",
                table: "ServiceAvailabilities");

            migrationBuilder.DropIndex(
                name: "IX_ServiceAvailabilities_ExtraServiceId",
                table: "ServiceAvailabilities");

            migrationBuilder.DropColumn(
                name: "ExtraServiceId",
                table: "ServiceAvailabilities");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAvailabilities_ServiceId",
                table: "ServiceAvailabilities",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceAvailabilities_ExtraServices_ServiceId",
                table: "ServiceAvailabilities",
                column: "ServiceId",
                principalTable: "ExtraServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceAvailabilities_ExtraServices_ServiceId",
                table: "ServiceAvailabilities");

            migrationBuilder.DropIndex(
                name: "IX_ServiceAvailabilities_ServiceId",
                table: "ServiceAvailabilities");

            migrationBuilder.AddColumn<int>(
                name: "ExtraServiceId",
                table: "ServiceAvailabilities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAvailabilities_ExtraServiceId",
                table: "ServiceAvailabilities",
                column: "ExtraServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceAvailabilities_ExtraServices_ExtraServiceId",
                table: "ServiceAvailabilities",
                column: "ExtraServiceId",
                principalTable: "ExtraServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
