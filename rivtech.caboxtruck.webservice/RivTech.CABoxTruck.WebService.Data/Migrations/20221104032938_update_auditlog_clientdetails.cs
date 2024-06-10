using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class update_auditlog_clientdetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Browser",
                table: "AuditLog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BrowserVersion",
                table: "AuditLog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceType",
                table: "AuditLog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "AuditLog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OS",
                table: "AuditLog",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Browser",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "BrowserVersion",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "DeviceType",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "OS",
                table: "AuditLog");
        }
    }
}
