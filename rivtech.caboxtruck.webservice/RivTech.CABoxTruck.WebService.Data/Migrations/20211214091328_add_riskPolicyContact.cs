using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class add_riskPolicyContact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubmissionSpecialistUserId",
                table: "BrokerInfo",
                newName: "AssistantUnderwriterId");

            migrationBuilder.CreateTable(
                name: "RiskPolicyContact",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RiskDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeId = table.Column<short>(type: "smallint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskPolicyContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskPolicyContact_Entity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiskPolicyContact_EntityId",
                table: "RiskPolicyContact",
                column: "EntityId");

            migrationBuilder.Sql(UPDATE_POLICYCONTACTS_DATA);
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskPolicyContact");

            migrationBuilder.RenameColumn(
                name: "AssistantUnderwriterId",
                table: "BrokerInfo",
                newName: "SubmissionSpecialistUserId");

            migrationBuilder.Sql(REVERT_POLICYCONTACTS_DATA);
        }

        const string UPDATE_POLICYCONTACTS_DATA = @"
            UPDATE dbo.PolicyContacts SET Position = 'Assistant Underwriter' WHERE Position = 'Submission Specialist'
        ";

        const string REVERT_POLICYCONTACTS_DATA = @"
            UPDATE dbo.PolicyContacts SET Position = 'Submission Specialist' WHERE Position = 'Assistant Underwriter'
        ";
    }
}
