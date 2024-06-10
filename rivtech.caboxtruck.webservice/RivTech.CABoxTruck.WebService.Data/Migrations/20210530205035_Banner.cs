using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Banner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banner",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Description = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    FileName = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    FilePath = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    PageLink = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RemovedBy = table.Column<long>(type: "bigint", nullable: false),
                    RemovedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banner", x => x.Id);
                });

            migrationBuilder.Sql(this.Data());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banner");
        }

        //https://stdocumentboxtruckdev.blob.core.windows.net should be replace by the other azure server url
        private string Data()
        {
            return @"insert into  Banner(Description,FileName,FilePath,PageLink,DisplayOrder,CreatedBy,CreatedDate,IsActive,RemovedBy,RemovedDate)
            values 
            ('First Image',	'first.jpg','https://stdocumentboxtruckdev.blob.core.windows.net/other-document/Banner/Images/first.jpg','https://stdocdev.blob.core.windows.net/other-document/Banner/Files/second.pdf',1,1,GETDATE(),1,1,NULL),
            ('Second Image','second.jpg','https://stdocumentboxtruckdev.blob.core.windows.net/other-document/Banner/Images/second.jpg','https://stdocdev.blob.core.windows.net/other-document/Banner/Files/first.docx',2,1,GETDATE(),1,1,NULL),
            ('Third Image',	'third.jpg','https://stdocumentboxtruckdev.blob.core.windows.net/other-document/Banner/Images/third.jpg','https://google.com',3,1,GETDATE(),1,1,NULL),
            ('Fourth Image','fourth.jpg','https://stdocumentboxtruckdev.blob.core.windows.net/other-document/Banner/Images/fourth.jpg','https://google.com',4,1,GETDATE(),1,1,NULL),
            ('Fifth Image',	'fifth.jpg','https://stdocumentboxtruckdev.blob.core.windows.net/other-document/Banner/Images/fifth.jpg','https://google.com',5,1,GETDATE(),1,1,NULL)";
        }

    }
}
