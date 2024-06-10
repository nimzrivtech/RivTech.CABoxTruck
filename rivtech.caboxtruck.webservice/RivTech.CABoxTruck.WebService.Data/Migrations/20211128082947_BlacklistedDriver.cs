using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class BlacklistedDriver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlacklistedDriver",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true),
                    LastName = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true),
                    DriverLicenseNumber = table.Column<string>(type: "varchar(130)", maxLength: 130, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Comments = table.Column<string>(type: "varchar(130)", maxLength: 130, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlacklistedDriver", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlacklistedDriver");
        }
    }
}
