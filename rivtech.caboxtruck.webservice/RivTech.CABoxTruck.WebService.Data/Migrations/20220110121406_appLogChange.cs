using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class appLogChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "AppLog");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppLog");

            migrationBuilder.DropColumn(
                name: "Method",
                table: "AppLog");

            migrationBuilder.DropColumn(
                name: "StatusCode",
                table: "AppLog");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "AppLog");

            migrationBuilder.RenameColumn(
                name: "JsonMessage",
                table: "AppLog",
                newName: "ResponseValues");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "AppLog",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AffectedColumns",
                table: "AppLog",
                type: "varchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewValues",
                table: "AppLog",
                type: "varchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldValues",
                table: "AppLog",
                type: "varchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryKey",
                table: "AppLog",
                type: "varchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestValues",
                table: "AppLog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TableName",
                table: "AppLog",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "AppLog",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "AppLog",
                type: "bigint",
                maxLength: 50,
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AffectedColumns",
                table: "AppLog");

            migrationBuilder.DropColumn(
                name: "NewValues",
                table: "AppLog");

            migrationBuilder.DropColumn(
                name: "OldValues",
                table: "AppLog");

            migrationBuilder.DropColumn(
                name: "PrimaryKey",
                table: "AppLog");

            migrationBuilder.DropColumn(
                name: "RequestValues",
                table: "AppLog");

            migrationBuilder.DropColumn(
                name: "TableName",
                table: "AppLog");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "AppLog");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AppLog");

            migrationBuilder.RenameColumn(
                name: "ResponseValues",
                table: "AppLog",
                newName: "JsonMessage");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "AppLog",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "AppLog",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "AppLog",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Method",
                table: "AppLog",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusCode",
                table: "AppLog",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "AppLog",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
