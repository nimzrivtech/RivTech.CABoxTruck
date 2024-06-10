using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class EndorsementOOS_Updates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EndorsementNumber",
                table: "RiskDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsProcessed",
                table: "RiskDetail",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SourceRiskDetailId",
                table: "RiskDetail",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VersionNumber",
                table: "RiskDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrentEndorsementNumber",
                table: "Risk",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentRiskDetailId",
                table: "Risk",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ForEndorsementRiskDetailId",
                table: "Risk",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EndorsementChanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RiskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EndorsementNumber = table.Column<int>(type: "int", nullable: false),
                    Changes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndorsementChanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiskPolicyPremiumCoverage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PremiumType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "date", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "date", nullable: true),
                    Premium_Annual = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    Premium_Prorated = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskPolicyPremiumCoverage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiskPolicyPremiumTransaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RiskDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PremiumType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PremiumChange = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    CurrentPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    PreviousPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskPolicyPremiumTransaction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiskVehiclePremiumCoverage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ALPremium_Annual = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    ALPremium_Prorated = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    PDPremium_Annual = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    PDPremium_Prorated = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    CargoPremium_Annual = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    CargoPremium_Prorated = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    CompFTPremium_Annual = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    CompFTPremium_Prorated = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    CollisionPremium_Annual = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    CollisionPremium_Prorated = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    RefrigerationPremium_Annual = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    RefrigerationPremium_Prorated = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    EffectiveDate = table.Column<DateTime>(type: "date", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "date", nullable: true),
                    Premium_Annual = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    Premium_Prorated = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskVehiclePremiumCoverage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiskVehiclePremiumTransaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RiskDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleMainId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PremiumChange = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    CurrentPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    PreviousPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    CurrentALPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    CurrentPDPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    CurrentCargoPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    CurrentCompPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    CurrentCollPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    CurrentRefPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskVehiclePremiumTransaction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiskPolicyPremium",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PremiumType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiskDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CoveragePremiumId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskPolicyPremium", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskPolicyPremium_RiskPolicyPremiumCoverage_CoveragePremiumId",
                        column: x => x.CoveragePremiumId,
                        principalTable: "RiskPolicyPremiumCoverage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RiskVehiclePremium",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleMainId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RiskDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CoveragePremiumId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskVehiclePremium", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskVehiclePremium_RiskVehiclePremiumCoverage_CoveragePremiumId",
                        column: x => x.CoveragePremiumId,
                        principalTable: "RiskVehiclePremiumCoverage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiskVehiclePremium_Vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiskPolicyPremium_CoveragePremiumId",
                table: "RiskPolicyPremium",
                column: "CoveragePremiumId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskVehiclePremium_CoveragePremiumId",
                table: "RiskVehiclePremium",
                column: "CoveragePremiumId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskVehiclePremium_VehicleId",
                table: "RiskVehiclePremium",
                column: "VehicleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EndorsementChanges");

            migrationBuilder.DropTable(
                name: "RiskPolicyPremium");

            migrationBuilder.DropTable(
                name: "RiskPolicyPremiumTransaction");

            migrationBuilder.DropTable(
                name: "RiskVehiclePremium");

            migrationBuilder.DropTable(
                name: "RiskVehiclePremiumTransaction");

            migrationBuilder.DropTable(
                name: "RiskPolicyPremiumCoverage");

            migrationBuilder.DropTable(
                name: "RiskVehiclePremiumCoverage");

            migrationBuilder.DropColumn(
                name: "EndorsementNumber",
                table: "RiskDetail");

            migrationBuilder.DropColumn(
                name: "IsProcessed",
                table: "RiskDetail");

            migrationBuilder.DropColumn(
                name: "SourceRiskDetailId",
                table: "RiskDetail");

            migrationBuilder.DropColumn(
                name: "VersionNumber",
                table: "RiskDetail");

            migrationBuilder.DropColumn(
                name: "CurrentEndorsementNumber",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "CurrentRiskDetailId",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "ForEndorsementRiskDetailId",
                table: "Risk");
        }
    }
}
