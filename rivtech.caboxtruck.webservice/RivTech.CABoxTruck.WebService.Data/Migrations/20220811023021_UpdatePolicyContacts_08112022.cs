using Microsoft.EntityFrameworkCore.Migrations;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class UpdatePolicyContacts_08112022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(UPDATE_SCRIPT);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

        const string UPDATE_SCRIPT = @"
            SET IDENTITY_INSERT [dbo].[PolicyContacts] ON
            DELETE FROM [dbo].[PolicyContacts] WHERE [Name] = 'John Bovill' and [Position] = 'Internal Underwriter'
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(50, 'John Bovill', 'jkbovill@rivpartners.com', 'Internal Underwriter')
            SET IDENTITY_INSERT [dbo].[PolicyContacts] OFF
        ";
    }
}
