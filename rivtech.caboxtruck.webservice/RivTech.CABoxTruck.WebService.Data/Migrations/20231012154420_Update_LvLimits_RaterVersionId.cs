using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLvLimitsRaterVersionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RaterVersionId",
                table: "LvLimits",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RaterVersionId",
                table: "LvLimits");

        }
    }
}
