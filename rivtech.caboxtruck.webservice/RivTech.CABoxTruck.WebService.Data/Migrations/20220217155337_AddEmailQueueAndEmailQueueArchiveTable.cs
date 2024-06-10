using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class AddEmailQueueAndEmailQueueArchiveTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailQueue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar", nullable: false),
                    BccAddress = table.Column<string>(type: "nvarchar", nullable: true),
                    CcAddress = table.Column<string>(type: "nvarchar", nullable: true),
                    FromAddress = table.Column<string>(type: "nvarchar", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentFileName = table.Column<string>(type: "nvarchar", nullable: true),
                    Attachment = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar", nullable: false),
                    IsSent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailQueue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailQueueArchive",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar", nullable: false),
                    BccAddress = table.Column<string>(type: "nvarchar", nullable: true),
                    CcAddress = table.Column<string>(type: "nvarchar", nullable: true),
                    FromAddress = table.Column<string>(type: "nvarchar", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentFileName = table.Column<string>(type: "nvarchar", nullable: true),
                    Attachment = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar", nullable: false),
                    IsSent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailQueueArchive", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailQueue");

            migrationBuilder.DropTable(
                name: "EmailQueueArchive");
        }
    }
}
