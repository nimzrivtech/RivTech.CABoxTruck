using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class UpdatePolicyContacts_04262022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[PolicyContacts]");
            migrationBuilder.Sql(UPDATE_POLICY_CONTACTS);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

        const string UPDATE_POLICY_CONTACTS = @"
            SET IDENTITY_INSERT [dbo].[PolicyContacts] ON
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(1, 'Ashley Bond', 'acbond@auw.com', 'Submission Specialist')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(2, 'Kurt Jensen', 'kajensen@auw.com', 'Submission Specialist')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(3, 'Taylor Redman', 'trredman@auw.com', 'Submission Specialist')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(4, 'Jeff Russell', 'jrussell@auw.com', 'Submission Specialist')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(5, 'Thomas Tegley III', 'tltegley@auw.com', 'Submission Specialist')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(6, 'Christina Millard', 'cjmillard@auw.com', 'PSR')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(7, 'Jonathan Runyan', 'jprunyan@auw.com', 'PSR')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(8, 'Bryan Burnett', 'bbburnett@auw.com', 'Team Leader')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(9, 'Jake Clark', 'jjclark@auw.com', 'Team Leader')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(10, 'Jeremy Curttright', 'jjcurttright@auw.com', 'Team Leader')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(11, 'Robert Fisher', 'rdfisher@auw.com', 'Team Leader')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(12, 'Justin Grebenick', 'jrgrebenick@auw.com', 'Team Leader')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(13, 'Janae Friis', 'jeosborne@auw.com', 'Team Leader')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(14, 'Matthew Puetz', 'mpuetz@auw.com', 'Team Leader')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(15, 'Debbie Bulik', 'dbulik@rivpartners.com', 'Internal Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(16, 'Mark Fintel', 'mefintel@rivpartners.com', 'Internal Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(17, 'Debbie Bulik', 'dbulik@rivpartners.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(18, 'Mark Fintel', 'mefintel@rivpartners.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(19, 'Kyle Murphy', 'kcmurphy@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(20, 'Sean Popejoy', 'sjpopejoy@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(21, 'Tyler Roenfeldt', 'tajroenfeldt@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(22, 'Pash Shinwari', 'pshinwari@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(23, 'Brian Siegert', 'blsiegert@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(24, 'Cori Tranisi', 'catranisi@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(25, 'Patrick Tvrdy', 'pstvrdy@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(26, 'Shane Ury', 'msury@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(27, 'Richard Chrostek', 'rechrostek@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(28, 'Jacob Christoffersen', 'jachristoffersen@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(29, 'Mike Jack', 'mdjack@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(30, 'Steve Dennis', 'srdennis@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(31, 'Scott Draper', 'jsdraper@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(32, 'Chad Hulse', 'cdhulse@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(33, 'Randy Jones', 'rcjones@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(34, 'Mike Kiser', 'mjkiser@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(35, 'Kellie Meisinger', 'kmmeisinger@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(36, 'Manny Menezes', 'mjmenezes@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(37, 'Ken Muckey', 'kamuckey@auw.com', 'Product Underwriter')
            SET IDENTITY_INSERT [dbo].[PolicyContacts] OFF
        ";
    }
}
