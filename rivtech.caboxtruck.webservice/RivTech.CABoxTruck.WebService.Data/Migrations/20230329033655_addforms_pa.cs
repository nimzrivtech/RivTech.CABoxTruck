using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addformspa : Migration
    {
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
            Insert into [Form] values('PennsylvaniaChanges','Pennsylvania Changes', 57, 1, 'CA 01 80 03 21', 'PA/Pennsylvania Changes.docx', 57, 0, 0, GETDATE(),1, 1)
            Insert into [Form] values('PennsylvaniaBasicFirstPartyBenefits','Pennsylvania Basic First Party Benefits', 58, 1, 'CA 22 37 03 06', 'PA/Pennsylvania Basic First Party Benefit.docx', 58, 0, 0, GETDATE(),1, 1)
            Insert into [Form] values('PennsylvaniaSplitUMCoverageLimitsNS','Pennsylvania Split Uninsured Motorists Coverage Limits - Nonstacked', 59, 1, 'CA 21 95 09 22', 'PA/Pennsylvania Split Uninsured Motorist Coverage - Nonstacked.docx', 59, 0, 0, GETDATE(),0, 1)
            Insert into [Form] values('PennsylvaniaSplitUIMCoverageLimitsNS','Pennsylvania Split Underinsured Motorists Coverage Limits - Nonstacked', 60, 1, 'CA 21 96 09 22', 'PA/Pennsylvania Split Underinsured Motorist Coverage - Nonstacked.docx', 60, 0, 0, GETDATE(),0, 1)
            Insert into [Form] values('PennsylvaniaUMCoverageLimitsNS','Pennsylvania Uninsured Motorists Coverage Limits - Nonstacked', 61, 1, 'CA 21 92 09 22', 'PA/Pennsylvania Uninsured Motorist Coverage - Nonstacked.docx', 61, 0, 0, GETDATE(),0, 1)
            Insert into [Form] values('PennsylvaniaUIMCoverageLimitsNS','Pennsylvania Underinsured Motorists Coverage Limits - Nonstacked', 62, 1, 'CA 21 93 09 22', 'PA/Pennsylvania Underinsured Motorist Coverage - Nonstacked.docx', 62, 0, 0, GETDATE(),0, 1)
            GO
        ";

        const string SQL_DELETE = @"
            Delete from [Form] where Id = 'PennsylvaniaChanges'
            Delete from [Form] where Id = 'PennsylvaniaBasicFirstPartyBenefits'
            Delete from [Form] where Id = 'PennsylvaniaSplitUMCoverageLimitsNS'
            Delete from [Form] where Id = 'PennsylvaniaSplitUIMCoverageLimitsNS'
            Delete from [Form] where Id = 'PennsylvaniaUMCoverageLimitsNS'
            Delete from [Form] where Id = 'PennsylvaniaUIMCoverageLimitsNS'
            GO
        ";
    }
}
