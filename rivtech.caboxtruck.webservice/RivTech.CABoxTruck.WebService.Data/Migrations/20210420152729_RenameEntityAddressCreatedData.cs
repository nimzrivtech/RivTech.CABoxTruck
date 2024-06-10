using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class RenameEntityAddressCreatedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AddedBy",
                table: "EntityAddress",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "AddProcessDate",
                table: "EntityAddress",
                newName: "CreatedDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "EntityAddress",
                newName: "AddProcessDate");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "EntityAddress",
                newName: "AddedBy");
        }
    }
}
