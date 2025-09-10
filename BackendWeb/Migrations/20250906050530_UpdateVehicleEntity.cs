using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendWeb.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVehicleEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "Plate",
                table: "Vehicles",
                newName: "StatusName");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Vehicles",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Vehicles",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Vehicles",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Vehicles",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DriverName",
                table: "Vehicles",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "FleetId",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FleetName",
                table: "Vehicles",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastServiceDate",
                table: "Vehicles",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicensePlate",
                table: "Vehicles",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Mileage",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextServiceDate",
                table: "Vehicles",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Vehicles",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_LicensePlate",
                table: "Vehicles",
                column: "LicensePlate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vehicles_LicensePlate",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "DriverName",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "FleetId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "FleetName",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "LastServiceDate",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "LicensePlate",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Mileage",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "NextServiceDate",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "StatusName",
                table: "Vehicles",
                newName: "Plate");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Vehicles",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Vehicles",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Vehicles",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
