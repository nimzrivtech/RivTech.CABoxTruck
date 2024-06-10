using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class risk_specific_destination : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RiskSpecificDestination",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    City = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    State = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    ZipCode = table.Column<string>(type: "varchar(10)", nullable: false),
                    TravelPercentage = table.Column<decimal>(type: "decimal(3, 2)", nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskSpecificDestination", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskSpecificDestination");
        }
    }
}
