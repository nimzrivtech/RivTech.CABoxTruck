using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Update_FileCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //// MigrationScript if Database is empty

            //migrationBuilder.DropForeignKey(
            //    name: "FK_FileUploadDocument_FileCategory_FileCategoryId",
            //    table: "FileUploadDocument");

            //migrationBuilder.DropTable(
            //    name: "FileCategory");

            //migrationBuilder.CreateTable(
            //    name: "FileCategory",
            //    columns: table => new
            //    {
            //        Id = table.Column<short>(type: "smallint", nullable: false),
            //        Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_FileCategory", x => x.Id);
            //    });

            //migrationBuilder.AddForeignKey(
            //    name: "FK_FileUploadDocument_FileCategory_FileCategoryId",
            //    table: "FileUploadDocument",
            //    column: "FileCategoryId",
            //    principalTable: "FileCategory",
            //    principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
