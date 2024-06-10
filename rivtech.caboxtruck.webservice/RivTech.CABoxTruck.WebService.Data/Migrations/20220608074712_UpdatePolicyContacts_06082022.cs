using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class UpdatePolicyContacts_06082022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(UPDATE_POLICY_CONTACTS);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

        const string UPDATE_POLICY_CONTACTS = @"
            SET IDENTITY_INSERT [dbo].[PolicyContacts] ON
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(41, 'Kent Polkinghorne', 'kjpolkinghorne@auw.com', 'Internal Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(42, 'Sara Kenkel', 'skenkel@auw.com', 'Product Underwriter')
            DELETE FROM [dbo].[PolicyContacts] WHERE [Name] = 'Mark Fintel' and [Position] = 'Internal Underwriter'
            SET IDENTITY_INSERT [dbo].[PolicyContacts] OFF
        ";
    }
}
