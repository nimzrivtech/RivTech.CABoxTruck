using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addformstxpart1newcabt29262926 : Migration
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
            Insert into[Form] values('TexasChangesCancellationandNonRenewal','Texas Changes - Cancellation and Non-Renewal', 41, 1, 'CA 02 43 11 13', 'TX/Texas Changes - Cancellation and Non-Renewal.docx', 41, 0, 0, GETDATE(),1, 1)
            Insert into[Form] values('TexasChanges','Texas Changes', 42, 1, 'CA 01 96 11 20', 'TX/Texas Changes.docx', 42, 0, 0, GETDATE(),1, 1)
            Insert into[Form] values('TexasPersonalInjuryProtection','Texas Personal Injury Protection Endorsement', 43, 1, 'CA 22 64 10 13', 'TX/Texas Personal Injury Protection Endorsement.docx', 43, 0, 0, GETDATE(),0, 1)
            Insert into[Form] values('TexasUMandUIMCoverage','Texas Uninsured/Underinsured Motorist Coverage', 44, 1, 'CA 21 09 10 13', 'TX/Texas UM and UIM Coverage.docx', 44, 0, 0, GETDATE(),0, 1)
            Insert into[Form] values('TexasSplitUninsuredandUnderinsuredMotoristCoverage','Texas Split Uninsured and Underinsured Motorist Coverage Form', 45, 1, 'CA 31 25 02 01', 'TX/Texas Split Uninsured and Underinsured Motorist Coverage Form.docx', 45, 0, 0, GETDATE(),0, 1)
            GO
        ";

        const string SQL_DELETE = @"
            Delete from [Form] where Id = 'TexasChangesCancellationandNonRenewal'
            Delete from [Form] where Id = 'TexasChanges'
            Delete from [Form] where Id = 'TexasPersonalInjuryProtection'
            Delete from [Form] where Id = 'TexasUMandUIMCoverage'
            Delete from [Form] where Id = 'TexasSplitUninsuredandUnderinsuredMotoristCoverage'
            GO
        ";
    }
}
