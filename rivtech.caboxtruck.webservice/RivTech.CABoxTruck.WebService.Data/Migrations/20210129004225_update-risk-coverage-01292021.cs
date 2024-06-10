using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class updateriskcoverage01292021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "CargoLimitSymbolId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "CollDeductibleSymbolId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "CompDeductibleSymbolId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "FireDeductibleSymbolId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "GLBILimitSymbolId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "LiabilityDeductibleSymbolId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "MedPayLimitSymbolId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "PIPLimitSymbolId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "RefCargoLimitSymbolId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "UIMBILimitSymbolId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "UIMPDLimitSymbolId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "UMBILimitSymbolId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "UMPDLimitSymbolId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CargoLimitSymbolId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "CollDeductibleSymbolId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "CompDeductibleSymbolId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "FireDeductibleSymbolId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "GLBILimitSymbolId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "LiabilityDeductibleSymbolId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "MedPayLimitSymbolId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "PIPLimitSymbolId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "RefCargoLimitSymbolId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "UIMBILimitSymbolId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "UIMPDLimitSymbolId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "UMBILimitSymbolId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "UMPDLimitSymbolId",
                table: "RiskCoverage");
        }
    }
}
