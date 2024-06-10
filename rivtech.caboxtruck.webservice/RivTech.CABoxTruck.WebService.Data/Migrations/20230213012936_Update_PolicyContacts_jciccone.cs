using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePolicyContactsjciccone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(UPDATE_POLICY_CONTACTS);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

        const string UPDATE_POLICY_CONTACTS = @"
            SET IDENTITY_INSERT [dbo].[PolicyContacts] ON
           
			 IF NOT EXISTS(SELECT [Name] FROM [dbo].[PolicyContacts] WHERE [Name]='Joe Ciccone' AND [Email]='jciccone@rivpartners.com' AND  [Position]='Internal Underwriter')
			 BEGIN
				INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(56, 'Joe Ciccone', 'jciccone@rivpartners.com', 'Internal Underwriter')
			 END

            SET IDENTITY_INSERT [dbo].[PolicyContacts] OFF
        ";
    
    }
}
