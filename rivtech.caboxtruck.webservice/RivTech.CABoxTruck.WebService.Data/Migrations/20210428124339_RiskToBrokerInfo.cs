using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class RiskToBrokerInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"select * into TempBrokerInfo from BrokerInfo
                                    delete from BrokerInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerInfo_RiskDetail_RiskDetailId",
                table: "BrokerInfo");

            migrationBuilder.DropIndex(
                name: "IX_BrokerInfo_RiskDetailId",
                table: "BrokerInfo");

            migrationBuilder.RenameColumn(
                name: "RiskDetailId",
                table: "BrokerInfo",
                newName: "RiskId");

            migrationBuilder.AlterColumn<string>(
                name: "SubmissionNumber",
                table: "Risk",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BrokerInfo_RiskId",
                table: "BrokerInfo",
                column: "RiskId",
                unique: true,
                filter: "[RiskId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerInfo_Risk_RiskId",
                table: "BrokerInfo",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(@"insert into BrokerInfo(Id, RiskId, AgencyId, AgentId, SubAgencyId, SubAgentId, YearsWithAgency, ReasonMoveId, EffectiveDate, ExpirationDate, IsIncumbentAgency, IsBrokeredAccount, IsMidtermMove)
                                    select TempBrokerInfo.Id, RiskDetail.RiskId, AgencyId, AgentId, SubAgencyId, SubAgentId, YearsWithAgency, ReasonMoveId, EffectiveDate, ExpirationDate, IsIncumbentAgency, IsBrokeredAccount, IsMidtermMove from TempBrokerInfo
                                    inner join RiskDetail on RiskDetail.Id = TempBrokerInfo.RiskDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrokerInfo_Risk_RiskId",
                table: "BrokerInfo");

            migrationBuilder.DropIndex(
                name: "IX_BrokerInfo_RiskId",
                table: "BrokerInfo");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "BrokerInfo",
                newName: "RiskDetailId");

            migrationBuilder.AlterColumn<string>(
                name: "SubmissionNumber",
                table: "Risk",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BrokerInfo_RiskDetailId",
                table: "BrokerInfo",
                column: "RiskDetailId",
                unique: true,
                filter: "[RiskDetailId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerInfo_RiskDetail_RiskDetailId",
                table: "BrokerInfo",
                column: "RiskDetailId",
                principalTable: "RiskDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
