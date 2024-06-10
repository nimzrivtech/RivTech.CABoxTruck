using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRiskCoverageSP33 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region RiskCoveragePremium
            migrationBuilder.AddColumn<decimal>(
                    name: "NonOwnedTrailerPremium",
                    table: "RiskCoveragePremium",
                    type: "decimal(18,2)",
                    nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TrailerInterchangeAL",
                table: "RiskCoveragePremium",
                type: "decimal(18,2)",
                nullable: true);
            #endregion

            #region RiskCoverage
            migrationBuilder.AddColumn<short>(
                    name: "NonOwnedTrailerLimitId",
                    table: "RiskCoverage",
                    type: "smallint",
                    nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "NonOwnedTrailerLimitSymbolId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "PollutionCovLimitId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "TrailerInterchangeLimitId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "TrailerInterchangeLimitSymbolId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "TrailerTypeLimitId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true); 
            #endregion
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            #region RiskCoveragePremium
            migrationBuilder.DropColumn(
                    name: "NonOwnedTrailerPremium",
                    table: "RiskCoveragePremium");

            migrationBuilder.DropColumn(
                name: "TrailerInterchangeAL",
                table: "RiskCoveragePremium");
            #endregion

            #region RiskCoverage
            migrationBuilder.DropColumn(
                    name: "NonOwnedTrailerLimitId",
                    table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "NonOwnedTrailerLimitSymbolId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "PollutionCovLimitId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "TrailerInterchangeLimitId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "TrailerInterchangeLimitSymbolId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "TrailerTypeLimitId",
                table: "RiskCoverage"); 
            #endregion
        }
    }
}
