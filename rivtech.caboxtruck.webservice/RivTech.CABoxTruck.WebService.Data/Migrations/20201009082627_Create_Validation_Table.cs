using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Create_Validation_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LvValidationType",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvValidationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Validation",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ValidationTypeId = table.Column<short>(nullable: false),
                    Value = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Validation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Validation_LvValidationType_ValidationTypeId",
                        column: x => x.ValidationTypeId,
                        principalTable: "LvValidationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionValidation",
                columns: table => new
                {
                    QuestionId = table.Column<short>(nullable: false),
                    ValidationId = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionValidation", x => new { x.QuestionId, x.ValidationId });
                    table.ForeignKey(
                        name: "FK_QuestionValidation_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionValidation_Validation_ValidationId",
                        column: x => x.ValidationId,
                        principalTable: "Validation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionValidation_ValidationId",
                table: "QuestionValidation",
                column: "ValidationId");

            migrationBuilder.CreateIndex(
                name: "IX_Validation_ValidationTypeId",
                table: "Validation",
                column: "ValidationTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionValidation");

            migrationBuilder.DropTable(
                name: "Validation");

            migrationBuilder.DropTable(
                name: "LvValidationType");
        }
    }
}
