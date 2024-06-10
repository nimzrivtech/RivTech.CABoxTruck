using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class adddatasettocontext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BackgroundCheckOption_RiskSpecificDriverHiringCriteria_RiskSpecificDriverHiringCriteriaId",
                table: "BackgroundCheckOption");

            migrationBuilder.DropForeignKey(
                name: "FK_DriverTrainingOption_RiskSpecificDriverHiringCriteria_RiskSpecificDriverHiringCriteriaId",
                table: "DriverTrainingOption");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskSpecificDriverInfo_MvrPullingFrequencyOption_MvrPullingFrequencyId",
                table: "RiskSpecificDriverInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MvrPullingFrequencyOption",
                table: "MvrPullingFrequencyOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DriverTrainingOption",
                table: "DriverTrainingOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BackgroundCheckOption",
                table: "BackgroundCheckOption");

            migrationBuilder.RenameTable(
                name: "MvrPullingFrequencyOption",
                newName: "MvrPullingFrequencyOptions");

            migrationBuilder.RenameTable(
                name: "DriverTrainingOption",
                newName: "DriverTrainingOptions");

            migrationBuilder.RenameTable(
                name: "BackgroundCheckOption",
                newName: "BackgroundCheckOptions");

            migrationBuilder.RenameIndex(
                name: "IX_DriverTrainingOption_RiskSpecificDriverHiringCriteriaId",
                table: "DriverTrainingOptions",
                newName: "IX_DriverTrainingOptions_RiskSpecificDriverHiringCriteriaId");

            migrationBuilder.RenameIndex(
                name: "IX_BackgroundCheckOption_RiskSpecificDriverHiringCriteriaId",
                table: "BackgroundCheckOptions",
                newName: "IX_BackgroundCheckOptions_RiskSpecificDriverHiringCriteriaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MvrPullingFrequencyOptions",
                table: "MvrPullingFrequencyOptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DriverTrainingOptions",
                table: "DriverTrainingOptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BackgroundCheckOptions",
                table: "BackgroundCheckOptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BackgroundCheckOptions_RiskSpecificDriverHiringCriteria_RiskSpecificDriverHiringCriteriaId",
                table: "BackgroundCheckOptions",
                column: "RiskSpecificDriverHiringCriteriaId",
                principalTable: "RiskSpecificDriverHiringCriteria",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DriverTrainingOptions_RiskSpecificDriverHiringCriteria_RiskSpecificDriverHiringCriteriaId",
                table: "DriverTrainingOptions",
                column: "RiskSpecificDriverHiringCriteriaId",
                principalTable: "RiskSpecificDriverHiringCriteria",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskSpecificDriverInfo_MvrPullingFrequencyOptions_MvrPullingFrequencyId",
                table: "RiskSpecificDriverInfo",
                column: "MvrPullingFrequencyId",
                principalTable: "MvrPullingFrequencyOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BackgroundCheckOptions_RiskSpecificDriverHiringCriteria_RiskSpecificDriverHiringCriteriaId",
                table: "BackgroundCheckOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_DriverTrainingOptions_RiskSpecificDriverHiringCriteria_RiskSpecificDriverHiringCriteriaId",
                table: "DriverTrainingOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskSpecificDriverInfo_MvrPullingFrequencyOptions_MvrPullingFrequencyId",
                table: "RiskSpecificDriverInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MvrPullingFrequencyOptions",
                table: "MvrPullingFrequencyOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DriverTrainingOptions",
                table: "DriverTrainingOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BackgroundCheckOptions",
                table: "BackgroundCheckOptions");

            migrationBuilder.RenameTable(
                name: "MvrPullingFrequencyOptions",
                newName: "MvrPullingFrequencyOption");

            migrationBuilder.RenameTable(
                name: "DriverTrainingOptions",
                newName: "DriverTrainingOption");

            migrationBuilder.RenameTable(
                name: "BackgroundCheckOptions",
                newName: "BackgroundCheckOption");

            migrationBuilder.RenameIndex(
                name: "IX_DriverTrainingOptions_RiskSpecificDriverHiringCriteriaId",
                table: "DriverTrainingOption",
                newName: "IX_DriverTrainingOption_RiskSpecificDriverHiringCriteriaId");

            migrationBuilder.RenameIndex(
                name: "IX_BackgroundCheckOptions_RiskSpecificDriverHiringCriteriaId",
                table: "BackgroundCheckOption",
                newName: "IX_BackgroundCheckOption_RiskSpecificDriverHiringCriteriaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MvrPullingFrequencyOption",
                table: "MvrPullingFrequencyOption",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DriverTrainingOption",
                table: "DriverTrainingOption",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BackgroundCheckOption",
                table: "BackgroundCheckOption",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BackgroundCheckOption_RiskSpecificDriverHiringCriteria_RiskSpecificDriverHiringCriteriaId",
                table: "BackgroundCheckOption",
                column: "RiskSpecificDriverHiringCriteriaId",
                principalTable: "RiskSpecificDriverHiringCriteria",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DriverTrainingOption_RiskSpecificDriverHiringCriteria_RiskSpecificDriverHiringCriteriaId",
                table: "DriverTrainingOption",
                column: "RiskSpecificDriverHiringCriteriaId",
                principalTable: "RiskSpecificDriverHiringCriteria",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskSpecificDriverInfo_MvrPullingFrequencyOption_MvrPullingFrequencyId",
                table: "RiskSpecificDriverInfo",
                column: "MvrPullingFrequencyId",
                principalTable: "MvrPullingFrequencyOption",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
