using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class VehicleSchemaUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MainId",
                table: "Vehicle",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "VehiclePremium",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RiskDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalAnnualPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    GrossProratedPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    NetProratedPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    PreviousId = table.Column<int>(type: "int", nullable: true),
                    AL_Annual = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    AL_GrossProrated = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    AL_NetProrated = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    PD_Annual = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    PD_GrossProrated = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    PD_NetProrated = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    Cargo_Annual = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    Cargo_GrossProrated = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    Cargo_NetProrated = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    CompFT_Annual = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    CompFT_GrossProrated = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    CompFT_NetProrated = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    Collision_Annual = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    Collision_GrossProrated = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    Collision_NetProrated = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    Refrigeration_Annual = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    Refrigeration_GrossProrated = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    Refrigeration_NetProrated = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehiclePremium", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehiclePremium_VehiclePremium_PreviousId",
                        column: x => x.PreviousId,
                        principalTable: "VehiclePremium",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePremium_PreviousId",
                table: "VehiclePremium",
                column: "PreviousId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehiclePremium");

            migrationBuilder.DropColumn(
                name: "MainId",
                table: "Vehicle");
        }
    }
}
