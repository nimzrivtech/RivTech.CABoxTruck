using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateLvLimitsseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SqlScripts.Seed_LvLimits_10102023);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
