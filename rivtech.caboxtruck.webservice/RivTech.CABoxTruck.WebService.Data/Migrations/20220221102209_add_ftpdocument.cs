using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class add_ftpdocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailQueue");

            migrationBuilder.DropTable(
                name: "EmailQueueArchive");

            migrationBuilder.CreateTable(
                name: "FTPDocument",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    RiskId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RiskDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BatchId = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Category = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    FileName = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    FilePath = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    IsUploaded = table.Column<bool>(type: "bit", nullable: false),
                    Source = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    IsCompiled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FTPDocument", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FTPDocumentTemporary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    RiskId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RiskDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BatchId = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Category = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    FileName = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    FilePath = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    IsUploaded = table.Column<bool>(type: "bit", nullable: false),
                    Source = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    IsCompiled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FTPDocumentTemporary", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FTPDocument_BatchId",
                table: "FTPDocument",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_FTPDocument_Category",
                table: "FTPDocument",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_FTPDocument_IsUploaded",
                table: "FTPDocument",
                column: "IsUploaded");

            migrationBuilder.CreateIndex(
                name: "IX_FTPDocumentTemporary_BatchId",
                table: "FTPDocumentTemporary",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_FTPDocumentTemporary_Category",
                table: "FTPDocumentTemporary",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_FTPDocumentTemporary_IsUploaded",
                table: "FTPDocumentTemporary",
                column: "IsUploaded");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FTPDocument");

            migrationBuilder.DropTable(
                name: "FTPDocumentTemporary");

            migrationBuilder.CreateTable(
                name: "EmailQueue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar", nullable: false),
                    Attachment = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    AttachmentFileName = table.Column<string>(type: "nvarchar", nullable: true),
                    BccAddress = table.Column<string>(type: "nvarchar", nullable: true),
                    CcAddress = table.Column<string>(type: "nvarchar", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromAddress = table.Column<string>(type: "nvarchar", nullable: true),
                    IsSent = table.Column<bool>(type: "bit", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar", nullable: false),
                    Type = table.Column<string>(type: "nvarchar", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    Attachment = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    AttachmentFileName = table.Column<string>(type: "nvarchar", nullable: true),
                    BccAddress = table.Column<string>(type: "nvarchar", nullable: true),
                    CcAddress = table.Column<string>(type: "nvarchar", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromAddress = table.Column<string>(type: "nvarchar", nullable: true),
                    IsSent = table.Column<bool>(type: "bit", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar", nullable: false),
                    Type = table.Column<string>(type: "nvarchar", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailQueueArchive", x => x.Id);
                });
        }
    }
}
