using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HostMaster.Backend.Migrations
{
    /// <inheritdoc />
    public partial class ReservationCustomerDocumentNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Customers_CustomerId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_CustomerId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_RoomId_StartDate",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Reservations",
                newName: "DocumentNumber");

            migrationBuilder.AddColumn<int>(
                name: "CustomerDocumentNumber",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CustomerDocumentNumber",
                table: "Reservations",
                column: "CustomerDocumentNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_RoomId_AccommodationId_StartDate_EndDate",
                table: "Reservations",
                columns: new[] { "RoomId", "AccommodationId", "StartDate", "EndDate" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Customers_CustomerDocumentNumber",
                table: "Reservations",
                column: "CustomerDocumentNumber",
                principalTable: "Customers",
                principalColumn: "DocumentNumber",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Customers_CustomerDocumentNumber",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_CustomerDocumentNumber",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_RoomId_AccommodationId_StartDate_EndDate",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "CustomerDocumentNumber",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "DocumentNumber",
                table: "Reservations",
                newName: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CustomerId",
                table: "Reservations",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_RoomId_StartDate",
                table: "Reservations",
                columns: new[] { "RoomId", "StartDate" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Customers_CustomerId",
                table: "Reservations",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "DocumentNumber",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
