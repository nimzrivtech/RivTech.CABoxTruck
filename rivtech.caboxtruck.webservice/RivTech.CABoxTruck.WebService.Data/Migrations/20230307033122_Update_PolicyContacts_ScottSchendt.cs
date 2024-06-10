using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePolicyContactsScottSchendt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(UPDATE_POLICY_CONTACTS);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

        const string UPDATE_POLICY_CONTACTS = @"
            IF NOT EXISTS(SELECT [Name] FROM [dbo].[PolicyContacts] WHERE [Name]='Scott Schendt' AND [Email]='seschendt@auw.com' AND  [Position]='Product Underwriter')
            BEGIN
	            SET IDENTITY_INSERT [dbo].[PolicyContacts] ON
	            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position], [IsAssignedTo]) 
	            VALUES(
		            (select top 1 ID from PolicyContacts order by Id desc) + 1, 
		            'Scott Schendt', 
		            'seschendt@auw.com', 
		            'Product Underwriter', 
		            0
	            )
	            SET IDENTITY_INSERT [dbo].[PolicyContacts] OFF
            END
        ";
    }
}
