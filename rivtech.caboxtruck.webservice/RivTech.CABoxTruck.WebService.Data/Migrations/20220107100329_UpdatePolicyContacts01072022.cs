using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class UpdatePolicyContacts01072022 : Migration
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
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(43, 'Bryan Burnett', 'bbburnett@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(44, 'Jake Clark', 'jjclark@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(45, 'Jeremy Curttright', 'jjcurttright@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(46, 'Robert Fisher', 'rdfisher@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(47, 'Justin Grebenick', 'jrgrebnick@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(48, 'Janae Osborne', 'jeosborne@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(49, 'Matthew Puetz', 'mpuetz@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(50, 'Kristi Solon', 'kdsolon@auw.com', 'Product Underwriter')
            SET IDENTITY_INSERT [dbo].[PolicyContacts] OFF
        ";
    }
}
