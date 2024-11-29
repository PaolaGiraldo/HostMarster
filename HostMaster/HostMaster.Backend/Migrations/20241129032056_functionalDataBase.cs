using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HostMaster.Backend.Migrations
{
    /// <inheritdoc />
    public partial class functionalDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Maintenances_RoomId_AccommodationId_StartDate_EndDate",
                table: "Maintenances");

            migrationBuilder.RenameColumn(
                name: "Like",
                table: "Opinions",
                newName: "Positives");

            migrationBuilder.RenameColumn(
                name: "Dislike",
                table: "Opinions",
                newName: "Negatives");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_RoomId",
                table: "Maintenances",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Maintenances_RoomId",
                table: "Maintenances");

            migrationBuilder.RenameColumn(
                name: "Positives",
                table: "Opinions",
                newName: "Like");

            migrationBuilder.RenameColumn(
                name: "Negatives",
                table: "Opinions",
                newName: "Dislike");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_RoomId_AccommodationId_StartDate_EndDate",
                table: "Maintenances",
                columns: new[] { "RoomId", "AccommodationId", "StartDate", "EndDate" },
                unique: true);
        }
    }
}
