using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class zipcode_updates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityZipCodeId",
                table: "Address");

            migrationBuilder.AlterColumn<int>(
                name: "PrimaryCityZipCodeOfOperationsId",
                table: "Risk",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Address",
                type: "varchar(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Address",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "County",
                table: "Address",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ZipCode",
                table: "Address",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "County",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Address");

            migrationBuilder.AlterColumn<short>(
                name: "PrimaryCityZipCodeOfOperationsId",
                table: "Risk",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Address",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(3)",
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CityZipCodeId",
                table: "Address",
                type: "int",
                nullable: true);
        }
    }
}
