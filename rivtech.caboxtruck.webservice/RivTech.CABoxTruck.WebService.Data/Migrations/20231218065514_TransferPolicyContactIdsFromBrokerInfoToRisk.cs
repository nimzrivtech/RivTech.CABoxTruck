using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class TransferPolicyContactIdsFromBrokerInfoToRisk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssistantUnderwriterId",
                table: "Risk",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InternalUWUserId",
                table: "Risk",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductUWUserId",
                table: "Risk",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PsrUserId",
                table: "Risk",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamLeadUserId",
                table: "Risk",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(SQL_STATEMENT);

            migrationBuilder.DropColumn(
                name: "AssistantUnderwriterId",
                table: "BrokerInfo");

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
                name: "TeamLeadUserId",
                table: "BrokerInfo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssistantUnderwriterId",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "InternalUWUserId",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "ProductUWUserId",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "PsrUserId",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "TeamLeadUserId",
                table: "Risk");

            migrationBuilder.AddColumn<int>(
                name: "AssistantUnderwriterId",
                table: "BrokerInfo",
                type: "int",
                nullable: true);

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
                name: "TeamLeadUserId",
                table: "BrokerInfo",
                type: "int",
                nullable: true);
        }

        const string SQL_STATEMENT = @"
            begin transaction

            declare @RiskId uniqueidentifier = null;
            declare @InternalUWUserId int = null;
            declare @ProductUWUserId int = null;
            declare @PsrUserId int = null;
            declare @AssistantUnderwriterId int = null;
            declare @TeamLeadUserId int = null;

            declare CUR_TEST CURSOR FAST_FORWARD FOR
	            SELECT RiskId, InternalUWUserId, ProductUWUserId, PsrUserId, AssistantUnderwriterId, TeamLeadUserId
	            FROM BrokerInfo

            open CUR_TEST
            FETCH NEXT FROM CUR_TEST INTO @RiskId, @InternalUWUserId, @ProductUWUserId, @PsrUserId, @AssistantUnderwriterId, @TeamLeadUserId

            --Loops on each RiskDetail in PolicyHistory
            while @@FETCH_STATUS = 0
            begin

	            UPDATE Risk
	            SET InternalUWUserId = @InternalUWUserId, 
		            ProductUWUserId = @ProductUWUserId, 
		            PsrUserId = @PsrUserId, 
		            AssistantUnderwriterId = @AssistantUnderwriterId, 
		            TeamLeadUserId = @TeamLeadUserId
	            WHERE Id = @RiskId

            FETCH NEXT FROM CUR_TEST INTO @RiskId, @InternalUWUserId, @ProductUWUserId, @PsrUserId, @AssistantUnderwriterId, @TeamLeadUserId
            end

            close CUR_TEST
            deallocate CUR_TEST

            UPDATE BrokerInfo
            SET InternalUWUserId = NULL, 
	            ProductUWUserId = NULL, 
	            PsrUserId = NULL, 
	            AssistantUnderwriterId = NULL, 
	            TeamLeadUserId = NULL

            commit transaction
        ";
    }
}
