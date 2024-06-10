using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class ChangeCancellationReasonTypeDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancellationReason",
                table: "Risk");

            migrationBuilder.AddColumn<string>(
                name: "CancellationReasonType",
                table: "Risk",
                type: "varchar(3)",
                maxLength: 3,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancellationReasonType",
                table: "Risk");

            migrationBuilder.AddColumn<int>(
                name: "CancellationReason",
                table: "Risk",
                type: "int",
                nullable: true);
        }
    }
}
