using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DieffeClean.Domain.Migrations
{
    public partial class NumberGuestsInReservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberGuests",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberGuests",
                table: "Reservations");
        }
    }
}
