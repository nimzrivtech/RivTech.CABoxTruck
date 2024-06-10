using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class updateentitysubsidiary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeOfBusiness",
                table: "EntitySubsidiary");

            migrationBuilder.AddColumn<short>(
                name: "TypeOfBusinessId",
                table: "EntitySubsidiary",
                type: "smallint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeOfBusinessId",
                table: "EntitySubsidiary");

            migrationBuilder.AddColumn<string>(
                name: "TypeOfBusiness",
                table: "EntitySubsidiary",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
