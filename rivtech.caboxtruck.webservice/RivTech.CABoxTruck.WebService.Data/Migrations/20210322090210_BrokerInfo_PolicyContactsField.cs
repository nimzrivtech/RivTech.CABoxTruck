using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class BrokerInfo_PolicyContactsField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "NumberOfInstallments",
                table: "RiskCoverage",
                type: "int",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InternalUWUserId",
                table: "BrokerInfo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductUWUserId",
                table: "BrokerInfo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PsrUserId",
                table: "BrokerInfo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubmissionSpecialistUserId",
                table: "BrokerInfo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamLeadUserId",
                table: "BrokerInfo",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "InternalUWUserId",
                table: "BrokerInfo");

            migrationBuilder.DropColumn(
                name: "ProductUWUserId",
                table: "BrokerInfo");

            migrationBuilder.DropColumn(
                name: "PsrUserId",
                table: "BrokerInfo");

            migrationBuilder.DropColumn(
                name: "SubmissionSpecialistUserId",
                table: "BrokerInfo");

            migrationBuilder.DropColumn(
                name: "TeamLeadUserId",
                table: "BrokerInfo");

            migrationBuilder.AlterColumn<short>(
                name: "NumberOfInstallments",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
