using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class add_forms_refrigeration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_UPDATE);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

        const string SQL_UPDATE = @"
            Insert into [Form] values ('RefrigerationCargoEndorsement','Refrigeration Cargo Endorsement', 37, 1, 'RPCA034', 'BT Refrigeration Cargo Endorsement.docx', 37, 0, 0, GETDATE(),0, 1)
            GO
        ";
    }
}
