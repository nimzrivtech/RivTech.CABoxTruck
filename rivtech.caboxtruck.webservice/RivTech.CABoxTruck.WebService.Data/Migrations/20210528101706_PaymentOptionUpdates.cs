using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class PaymentOptionUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalAmountDueFinanced",
                table: "RiskCoveragePremium",
                newName: "TotalAmountDueInstallment");

            migrationBuilder.RenameColumn(
                name: "DepositAmount",
                table: "RiskCoverage",
                newName: "DepositPremium");

            migrationBuilder.AddColumn<decimal>(
                name: "DepositAmount",
                table: "RiskCoveragePremium",
                type: "decimal(12,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PerInstallmentFee",
                table: "RiskCoveragePremium",
                type: "decimal(12,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PerInstallmentPremium",
                table: "RiskCoveragePremium",
                type: "decimal(12,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PerInstallmentTax",
                table: "RiskCoveragePremium",
                type: "decimal(12,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepositAmount",
                table: "RiskCoveragePremium");

            migrationBuilder.DropColumn(
                name: "PerInstallmentFee",
                table: "RiskCoveragePremium");

            migrationBuilder.DropColumn(
                name: "PerInstallmentPremium",
                table: "RiskCoveragePremium");

            migrationBuilder.DropColumn(
                name: "PerInstallmentTax",
                table: "RiskCoveragePremium");

            migrationBuilder.RenameColumn(
                name: "TotalAmountDueInstallment",
                table: "RiskCoveragePremium",
                newName: "TotalAmountDueFinanced");

            migrationBuilder.RenameColumn(
                name: "DepositPremium",
                table: "RiskCoverage",
                newName: "DepositAmount");
        }
    }
}
