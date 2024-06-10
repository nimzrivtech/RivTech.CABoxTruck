using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Create_Question_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LvQuestionCategory",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvQuestionCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LvQuestionSection",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvQuestionSection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LvQuestionType",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvQuestionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LvRiskResponseType",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvRiskResponseType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramId = table.Column<short>(type: "smallint", nullable: true),
                    StateId = table.Column<short>(type: "smallint", nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    ParentQuestionId = table.Column<short>(nullable: true),
                    CategoryId = table.Column<short>(nullable: false),
                    SectionId = table.Column<short>(nullable: false),
                    Order = table.Column<short>(type: "smallint", nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_LvQuestionCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "LvQuestionCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Question_Question_ParentQuestionId",
                        column: x => x.ParentQuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Question_LvQuestionSection_SectionId",
                        column: x => x.SectionId,
                        principalTable: "LvQuestionSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionConfig",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<short>(nullable: false),
                    QuestionTypeId = table.Column<short>(nullable: false),
                    Label = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    DefaultValue = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    ResponseTypeId = table.Column<short>(nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionConfig_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionConfig_LvQuestionType_QuestionTypeId",
                        column: x => x.QuestionTypeId,
                        principalTable: "LvQuestionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionConfig_LvRiskResponseType_ResponseTypeId",
                        column: x => x.ResponseTypeId,
                        principalTable: "LvRiskResponseType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskResponse",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RiskId = table.Column<Guid>(nullable: true),
                    QuestionId = table.Column<short>(nullable: false),
                    ResponseValue = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskResponse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskResponse_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiskResponse_Risk_RiskId",
                        column: x => x.RiskId,
                        principalTable: "Risk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Question_CategoryId",
                table: "Question",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_ParentQuestionId",
                table: "Question",
                column: "ParentQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_SectionId",
                table: "Question",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionConfig_QuestionId",
                table: "QuestionConfig",
                column: "QuestionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionConfig_QuestionTypeId",
                table: "QuestionConfig",
                column: "QuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionConfig_ResponseTypeId",
                table: "QuestionConfig",
                column: "ResponseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskResponse_QuestionId",
                table: "RiskResponse",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskResponse_RiskId",
                table: "RiskResponse",
                column: "RiskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionConfig");

            migrationBuilder.DropTable(
                name: "RiskResponse");

            migrationBuilder.DropTable(
                name: "LvQuestionType");

            migrationBuilder.DropTable(
                name: "LvRiskResponseType");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "LvQuestionCategory");

            migrationBuilder.DropTable(
                name: "LvQuestionSection");
        }
    }
}
