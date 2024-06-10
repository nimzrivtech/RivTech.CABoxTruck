using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Update_QuestionRiskResponse_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"delete dbo.Question");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskResponse_LvRiskResponseType_RiskResponseTypeId",
                table: "RiskResponse");

            migrationBuilder.DropTable(
                name: "QuestionConfig");

            migrationBuilder.DropTable(
                name: "LvQuestionType");

            migrationBuilder.DropIndex(
                name: "IX_RiskResponse_RiskResponseTypeId",
                table: "RiskResponse");

            migrationBuilder.DropColumn(
                name: "RiskResponseTypeId",
                table: "RiskResponse");

            migrationBuilder.AlterColumn<short>(
                name: "YearsInPlace",
                table: "RiskSpecificSafetyDevice",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<string>(
                name: "UnitDescription",
                table: "RiskSpecificSafetyDevice",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "PercentageOfFleet",
                table: "RiskSpecificSafetyDevice",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<bool>(
                name: "IsInPlace",
                table: "RiskSpecificSafetyDevice",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<short>(
                name: "RiskResponseTypeId",
                table: "Question",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateIndex(
                name: "IX_Question_RiskResponseTypeId",
                table: "Question",
                column: "RiskResponseTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_LvRiskResponseType_RiskResponseTypeId",
                table: "Question",
                column: "RiskResponseTypeId",
                principalTable: "LvRiskResponseType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_LvRiskResponseType_RiskResponseTypeId",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_RiskResponseTypeId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "RiskResponseTypeId",
                table: "Question");

            migrationBuilder.AlterColumn<short>(
                name: "YearsInPlace",
                table: "RiskSpecificSafetyDevice",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UnitDescription",
                table: "RiskSpecificSafetyDevice",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "PercentageOfFleet",
                table: "RiskSpecificSafetyDevice",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsInPlace",
                table: "RiskSpecificSafetyDevice",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<short>(
                name: "RiskResponseTypeId",
                table: "RiskResponse",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateTable(
                name: "LvQuestionType",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvQuestionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionConfig",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DefaultValue = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    Label = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    QuestionId = table.Column<short>(type: "smallint", nullable: false),
                    QuestionTypeId = table.Column<short>(type: "smallint", nullable: false),
                    ResponseTypeId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionConfig", x => x.Id);
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
                    table.ForeignKey(
                        name: "FK_QuestionConfig_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiskResponse_RiskResponseTypeId",
                table: "RiskResponse",
                column: "RiskResponseTypeId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_RiskResponse_LvRiskResponseType_RiskResponseTypeId",
                table: "RiskResponse",
                column: "RiskResponseTypeId",
                principalTable: "LvRiskResponseType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
