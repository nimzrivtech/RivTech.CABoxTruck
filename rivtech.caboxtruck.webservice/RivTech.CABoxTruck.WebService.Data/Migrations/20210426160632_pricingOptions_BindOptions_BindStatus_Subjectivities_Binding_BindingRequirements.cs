using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class pricingOptions_BindOptions_BindStatus_Subjectivities_Binding_BindingRequirements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SubmissionNumber",
                table: "Risk",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "BindOptions",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BindOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BindStatus",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BindStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PricingOptions",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricingOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjectivities",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjectivities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Binding",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BindOptionId = table.Column<short>(type: "smallint", nullable: true),
                    PricingOptionId = table.Column<short>(type: "smallint", nullable: true),
                    CreditOfficeId = table.Column<short>(type: "smallint", nullable: true),
                    SurplusLIneNum = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RiskDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Binding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Binding_BindOptions_BindOptionId",
                        column: x => x.BindOptionId,
                        principalTable: "BindOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Binding_PricingOptions_PricingOptionId",
                        column: x => x.PricingOptionId,
                        principalTable: "PricingOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Binding_RiskDetail_RiskDetailId",
                        column: x => x.RiskDetailId,
                        principalTable: "RiskDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BindingRequirements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectivitiesId = table.Column<short>(type: "smallint", nullable: true),
                    IsPreBind = table.Column<bool>(type: "bit", nullable: true),
                    BindStatusId = table.Column<short>(type: "smallint", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    RelevantDocumentDesc = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    RiskDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BindingRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BindingRequirements_BindStatus_BindStatusId",
                        column: x => x.BindStatusId,
                        principalTable: "BindStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BindingRequirements_RiskDetail_RiskDetailId",
                        column: x => x.RiskDetailId,
                        principalTable: "RiskDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BindingRequirements_Subjectivities_SubjectivitiesId",
                        column: x => x.SubjectivitiesId,
                        principalTable: "Subjectivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Binding_BindOptionId",
                table: "Binding",
                column: "BindOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Binding_PricingOptionId",
                table: "Binding",
                column: "PricingOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Binding_RiskDetailId",
                table: "Binding",
                column: "RiskDetailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BindingRequirements_BindStatusId",
                table: "BindingRequirements",
                column: "BindStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_BindingRequirements_RiskDetailId",
                table: "BindingRequirements",
                column: "RiskDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_BindingRequirements_SubjectivitiesId",
                table: "BindingRequirements",
                column: "SubjectivitiesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Binding");

            migrationBuilder.DropTable(
                name: "BindingRequirements");

            migrationBuilder.DropTable(
                name: "BindOptions");

            migrationBuilder.DropTable(
                name: "PricingOptions");

            migrationBuilder.DropTable(
                name: "BindStatus");

            migrationBuilder.DropTable(
                name: "Subjectivities");

            migrationBuilder.AlterColumn<string>(
                name: "SubmissionNumber",
                table: "Risk",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
