using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class update_risknotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RiskNotes",
                type: "varchar(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RiskNotes",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldNullable: true);
        }
    }
}
