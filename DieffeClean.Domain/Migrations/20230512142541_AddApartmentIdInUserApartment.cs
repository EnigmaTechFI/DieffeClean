using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DieffeClean.Domain.Migrations
{
    public partial class AddApartmentIdInUserApartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserApartments_Apartments_ApartmentId",
                table: "UserApartments");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApartmentId",
                table: "UserApartments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserApartments_Apartments_ApartmentId",
                table: "UserApartments",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserApartments_Apartments_ApartmentId",
                table: "UserApartments");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApartmentId",
                table: "UserApartments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_UserApartments_Apartments_ApartmentId",
                table: "UserApartments",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id");
        }
    }
}
