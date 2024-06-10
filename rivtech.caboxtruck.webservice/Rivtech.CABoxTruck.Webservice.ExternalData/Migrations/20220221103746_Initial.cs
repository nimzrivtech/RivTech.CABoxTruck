using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.ExternalData.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailQueueArchives",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    BccAddress = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    CcAddress = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    FromAddress = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentFileName = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attachment = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    IsSent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailQueueArchives", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailQueues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    BccAddress = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    CcAddress = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    FromAddress = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentFileName = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attachment = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    IsSent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailQueues", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailQueueArchives");

            migrationBuilder.DropTable(
                name: "EmailQueues");
        }
    }
}
