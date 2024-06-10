using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVehiclePdChangeIdentifier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsManualCollisionLimit",
                table: "Vehicle",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsManualCoverageFtLimit",
                table: "Vehicle",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsManualCollisionLimit",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "IsManualCoverageFtLimit",
                table: "Vehicle");
        }
    }
}
