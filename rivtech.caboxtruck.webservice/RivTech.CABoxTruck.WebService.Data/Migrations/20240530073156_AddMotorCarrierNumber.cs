using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMotorCarrierNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInsuredHasMotorCarrierNo",
                table: "RiskDetail",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "McNumber",
                table: "Entity",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInsuredHasMotorCarrierNo",
                table: "RiskDetail");

            migrationBuilder.DropColumn(
                name: "McNumber",
                table: "Entity");
        }
    }
}
