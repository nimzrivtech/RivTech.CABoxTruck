using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addformspab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_INSERT);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_DELETE);
        }

        const string SQL_INSERT = @"
            Insert into [Form] values('PennsylvaniaAddCombFirstPartyBenefitsEndorsement','Pennsylvania Added and Combination First-Party Benefits Endorsement', 63, 1, 'CA 22 38 10 13', 'PA/Pennsylvania Added and Combination First Party Benefits Endorsement.docx', 63, 0, 0, GETDATE(),0, 0)
            GO
        ";

        const string SQL_DELETE = @"
            Delete from [Form] where Id = 'PennsylvaniaAddCombFirstPartyBenefitsEndorsement'
            GO
        ";
    }
}
