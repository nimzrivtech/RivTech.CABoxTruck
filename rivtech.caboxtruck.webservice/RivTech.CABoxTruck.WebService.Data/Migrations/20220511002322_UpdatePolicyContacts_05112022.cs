using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class UpdatePolicyContacts_05112022 : Migration
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
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(38, 'Jason Dennis', 'jmdennis@auw.com', 'Submission Specialist')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(39, 'Matthew Marko', 'mdmarko@auw.com', 'Submission Specialist')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(40, 'Graham Stone', 'gjstone@auw.com', 'Submission Specialist')
            SET IDENTITY_INSERT [dbo].[PolicyContacts] OFF
        ";
    }
}
