using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Add_FTPDocumentFilesCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FTPDocumentFilesCount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegularCount = table.Column<int>(type: "int", nullable: true),
                    InvoiceCount = table.Column<int>(type: "int", nullable: true),
                    CertifiedCount = table.Column<int>(type: "int", nullable: true),
                    FTPSentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FTPDocumentFilesCount", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FTPDocumentFilesCount");
        }
    }
}
