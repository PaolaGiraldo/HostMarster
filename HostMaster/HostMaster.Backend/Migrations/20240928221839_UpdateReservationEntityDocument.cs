using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HostMaster.Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReservationEntityDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReservationRooms_ReservationId_RoomId",
                table: "ReservationRooms");

            migrationBuilder.AddColumn<int>(
                name: "CustomerDocument",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Document = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserType = table.Column<int>(type: "int", maxLength: 200, nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationRooms_ReservationId",
                table: "ReservationRooms",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_DocumentNumber",
                table: "Customers",
                column: "DocumentNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CityId",
                table: "Users",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Document",
                table: "Users",
                column: "Document",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_ReservationRooms_ReservationId",
                table: "ReservationRooms");

            migrationBuilder.DropIndex(
                name: "IX_Customers_DocumentNumber",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CustomerDocument",
                table: "Reservations");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationRooms_ReservationId_RoomId",
                table: "ReservationRooms",
                columns: new[] { "ReservationId", "RoomId" },
                unique: true);
        }
    }
}
