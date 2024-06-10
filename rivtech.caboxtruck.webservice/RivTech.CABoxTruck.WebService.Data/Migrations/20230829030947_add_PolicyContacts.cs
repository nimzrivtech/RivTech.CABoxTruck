using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addPolicyContacts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(UPDATE_POLICY_CONTACTS);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(DELETE_POLICY_CONTACTS);
        }

        const string UPDATE_POLICY_CONTACTS = @"
            IF NOT EXISTS(SELECT [Name] FROM [dbo].[PolicyContacts] WHERE [Name]='Jacob Christoffersen' AND [Email]='jachristoffersen@auw.com' 
                        AND  [Position]='Team Leader')
            BEGIN
	            SET IDENTITY_INSERT [dbo].[PolicyContacts] ON
	            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position], [IsAssignedTo]) 
	            VALUES(
		            (select top 1 ID from PolicyContacts order by Id desc) + 1, 
		            'Jacob Christoffersen', 
		            'jachristoffersen@auw.com', 
		            'Team Leader', 
		            0
	            )
	            SET IDENTITY_INSERT [dbo].[PolicyContacts] OFF
            END

            IF NOT EXISTS(SELECT [Name] FROM [dbo].[PolicyContacts] WHERE [Name]='Amy Brown' AND [Email]='aebrown@auw.com' 
                        AND  [Position]='Product Underwriter')
            BEGIN
	            SET IDENTITY_INSERT [dbo].[PolicyContacts] ON
	            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position], [IsAssignedTo]) 
	            VALUES(
		            (select top 1 ID from PolicyContacts order by Id desc) + 1, 
		            'Amy Brown', 
		            'aebrown@auw.com', 
		            'Product Underwriter', 
		            0
	            )
	            SET IDENTITY_INSERT [dbo].[PolicyContacts] OFF
            END

            IF NOT EXISTS(SELECT [Name] FROM [dbo].[PolicyContacts] WHERE [Name]='LeAnna Garcia' AND [Email]='lmgarcia@auw.com' 
                        AND  [Position]='Product Underwriter')
            BEGIN
	            SET IDENTITY_INSERT [dbo].[PolicyContacts] ON
	            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position], [IsAssignedTo]) 
	            VALUES(
		            (select top 1 ID from PolicyContacts order by Id desc) + 1, 
		            'LeAnna Garcia', 
		            'lmgarcia@auw.com', 
		            'Product Underwriter', 
		            0
	            )
	            SET IDENTITY_INSERT [dbo].[PolicyContacts] OFF
            END
        ";

        const string DELETE_POLICY_CONTACTS = @"
            delete from PolicyContacts where Id in (63,64,65)
        ";
    }
}
