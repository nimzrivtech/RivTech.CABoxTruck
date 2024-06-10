using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class fileupload_addField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RemovedDate",
                table: "FileUploadDocument",
                newName: "DeletedDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "FileUploadDocument",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "FileUploadDocument");

            migrationBuilder.RenameColumn(
                name: "DeletedDate",
                table: "FileUploadDocument",
                newName: "RemovedDate");
        }
    }
}
