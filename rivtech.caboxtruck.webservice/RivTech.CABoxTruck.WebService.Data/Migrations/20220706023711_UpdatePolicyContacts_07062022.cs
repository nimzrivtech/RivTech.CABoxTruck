using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class UpdatePolicyContacts_07062022 : Migration
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
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(43, 'Joseph Froistad', 'jmfroistad@auw.com', 'Internal Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(44, 'Jason Dennis', 'jmdennis@auw.com', 'Internal Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(45, 'Christopher Guzallis', 'cguzallis@auw.com', 'Internal Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(46, 'Sarah Lowman', 'smlowman@auw.com', 'Internal Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(47, 'Marissa Niemoller', 'maniemoller@auw.com', 'Internal Underwriter')
            DELETE FROM [dbo].[PolicyContacts] WHERE [Name] = 'Kent Polkinghorne' and [Position] = 'Internal Underwriter'
            SET IDENTITY_INSERT [dbo].[PolicyContacts] OFF
        ";
    }
}
