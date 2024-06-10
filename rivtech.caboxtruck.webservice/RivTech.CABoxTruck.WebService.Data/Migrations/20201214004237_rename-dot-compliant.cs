using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class renamedotcompliant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AreDriversProperlyLicensedDOTcompliant",
                table: "RiskSpecificDriverHiringCriteria",
                newName: "AreDriversProperlyLicensedDotCompliant");

            migrationBuilder.AlterColumn<short>(
                name: "MvrPullingFrequencyId",
                table: "RiskSpecificDriverInfo",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "MvrPullingFrequencyOption",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MvrPullingFrequencyOption", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiskSpecificDriverInfo_MvrPullingFrequencyId",
                table: "RiskSpecificDriverInfo",
                column: "MvrPullingFrequencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskSpecificDriverInfo_MvrPullingFrequencyOption_MvrPullingFrequencyId",
                table: "RiskSpecificDriverInfo",
                column: "MvrPullingFrequencyId",
                principalTable: "MvrPullingFrequencyOption",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskSpecificDriverInfo_MvrPullingFrequencyOption_MvrPullingFrequencyId",
                table: "RiskSpecificDriverInfo");

            migrationBuilder.DropTable(
                name: "MvrPullingFrequencyOption");

            migrationBuilder.DropIndex(
                name: "IX_RiskSpecificDriverInfo_MvrPullingFrequencyId",
                table: "RiskSpecificDriverInfo");

            migrationBuilder.RenameColumn(
                name: "AreDriversProperlyLicensedDotCompliant",
                table: "RiskSpecificDriverHiringCriteria",
                newName: "AreDriversProperlyLicensedDOTcompliant");

            migrationBuilder.AlterColumn<int>(
                name: "MvrPullingFrequencyId",
                table: "RiskSpecificDriverInfo",
                type: "int",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);
        }
    }
}
