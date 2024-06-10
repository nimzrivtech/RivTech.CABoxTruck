using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class UpdatePolicyContacts : Migration
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
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(1, 'Ashley Bond', 'acbond@auw.com', 'Assistant Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(2, 'Graham Stone', 'gjstone@auw.com', 'Assistant Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(3, 'Matthew Marko', 'mdmarko@auw.com', 'Assistant Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(4, 'Taylor Redman', 'trredman@auw.com', 'Assistant Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(5, 'Jeff Russell', 'jrussel@auw.com', 'Assistant Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(6, 'Thomas Tegley III', 'tltegley@auw.com', 'Assistant Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(7, 'Christina Millard', 'cjmillard@auw.com', 'PSR')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(8, 'Jonathan Runyan', 'jprunyan@auw.com', 'PSR')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(9, 'Bryan Burnett', 'bbburnett@auw.com', 'Team Leader')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(10, 'Jake Clark', 'jjclark@auw.com', 'Team Leader')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(11, 'Jeremy Curttright', 'jjcurttright@auw.com', 'Team Leader')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(12, 'Robert Fisher', 'rdfisher@auw.com', 'Team Leader')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(13, 'Justin Grebenick', 'jrgrebnick@auw.com', 'Team Leader')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(14, 'Janae Osborne', 'jeosborne@auw.com', 'Team Leader')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(15, 'Matthew Puetz', 'mpuetz@auw.com', 'Team Leader')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(16, 'Kristi Solon', 'kdsolon@auw.com', 'Team Leader')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(17, 'Debbie Bulik', 'dbulik@rivpartners.com', 'Internal Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(18, 'Kent Polkinghorne', 'kjpolkinghorne@auw.com', 'Internal Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(19, 'Debbie Bulik', 'dbulik@rivpartners.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(20, 'Mike Kiser', 'mjkiser@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(21, 'Scott Schendt', 'seschendt@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(22, 'Cori Tranisi', 'catranisi@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(23, 'Chance Frey', 'crfrey@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(24, 'Shane Ury', 'msury@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(25, 'Joey Gross', 'jagross@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(26, 'Kirby Johnson', 'kljohnson@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(27, 'Sean Popejoy', 'sjpopejoy@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(28, 'Chris Wood', 'cawood@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(29, 'Randy Jones', 'rcjones@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(30, 'Kellie Meisinger', 'kmmeisinger@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(31, 'Ken Muckey', 'kamuckey@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(32, 'Michael Jack', 'mdjack@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(33, 'Kipp Kester', 'klkester@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(34, 'Jacob Christoffersen', 'jachristofferson@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(35, 'Tyler Roenfeldt', 'tajroenfeldt@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(36, 'Patrick Tvrdy', 'pstvrdy@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(37, 'Richard Chrostek', 'rechrostek@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(38, 'Kyle Murphy', 'kcmurphy@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(39, 'Brian Siegert', 'blsiegert@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(40, 'Chad Hulse', 'cdhulse@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(41, 'Zach Miller', 'jzmiller@auw.com', 'Product Underwriter')
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position]) VALUES(42, 'Manny Menezes', 'mjmenezes@auw.com', 'Product Underwriter')
            SET IDENTITY_INSERT [dbo].[PolicyContacts] OFF
        ";
    }
}
