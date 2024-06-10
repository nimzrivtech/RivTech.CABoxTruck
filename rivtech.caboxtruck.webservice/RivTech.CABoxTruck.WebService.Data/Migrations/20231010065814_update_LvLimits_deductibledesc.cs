using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateLvLimitsdeductibledesc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Deductible",
                table: "LvLimits",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LvLimits",
                type: "varchar(100)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deductible",
                table: "LvLimits");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "LvLimits");
        }
    }
}
