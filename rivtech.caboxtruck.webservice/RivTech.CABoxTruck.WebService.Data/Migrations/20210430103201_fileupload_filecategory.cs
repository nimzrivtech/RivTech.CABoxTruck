using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class fileupload_filecategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileCategory",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileUploadDocument",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    tableRefId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileCategoryId = table.Column<short>(type: "smallint", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    IsConfidential = table.Column<bool>(type: "bit", nullable: true),
                    IsUploaded = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    RemovedBy = table.Column<long>(type: "bigint", nullable: true),
                    RemovedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUploadDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileUploadDocument_FileCategory_FileCategoryId",
                        column: x => x.FileCategoryId,
                        principalTable: "FileCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileUploadDocument_FileCategoryId",
                table: "FileUploadDocument",
                column: "FileCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileUploadDocument");

            migrationBuilder.DropTable(
                name: "FileCategory");
        }
    }
}
