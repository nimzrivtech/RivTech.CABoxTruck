using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class UpdatePolicyContacts_08172022 : Migration
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
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(51, 'Vincent DiPolito', 'vdipolito@rivpartners.com', 'Internal Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(52, 'Marchelle Thurman', 'mthurman@rivpartners.com', 'Internal Underwriter')
            SET IDENTITY_INSERT [dbo].[PolicyContacts] OFF
        ";
    }
}
