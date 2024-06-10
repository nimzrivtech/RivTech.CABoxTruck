using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class UpdateApplicantInterestsClaimsFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAutoLiability",
                table: "RiskAdditionalInterest",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsGeneralLiability",
                table: "RiskAdditionalInterest",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SocialSecurityNumber",
                table: "Entity",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CargoClaimCount",
                table: "ClaimsHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CargoNetLoss",
                table: "ClaimsHistory",
                type: "decimal(12,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GeneralLiabilityClaimCount",
                table: "ClaimsHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GeneralLiabilityNetLoss",
                table: "ClaimsHistory",
                type: "decimal(12,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhysicalDamageClaimCount",
                table: "ClaimsHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PhysicalDamageNetLoss",
                table: "ClaimsHistory",
                type: "decimal(12,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAutoLiability",
                table: "RiskAdditionalInterest");

            migrationBuilder.DropColumn(
                name: "IsGeneralLiability",
                table: "RiskAdditionalInterest");

            migrationBuilder.DropColumn(
                name: "SocialSecurityNumber",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "CargoClaimCount",
                table: "ClaimsHistory");

            migrationBuilder.DropColumn(
                name: "CargoNetLoss",
                table: "ClaimsHistory");

            migrationBuilder.DropColumn(
                name: "GeneralLiabilityClaimCount",
                table: "ClaimsHistory");

            migrationBuilder.DropColumn(
                name: "GeneralLiabilityNetLoss",
                table: "ClaimsHistory");

            migrationBuilder.DropColumn(
                name: "PhysicalDamageClaimCount",
                table: "ClaimsHistory");

            migrationBuilder.DropColumn(
                name: "PhysicalDamageNetLoss",
                table: "ClaimsHistory");
        }
    }
}
