using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePolicyContactsAddIsAssignedTo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedToId",
                table: "PolicyContacts",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(UPDATE_SCRIPT);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedToId",
                table: "PolicyContacts");
        }

        const string UPDATE_SCRIPT = @"
            UPDATE [PolicyContacts] SET [AssignedToId] = 1 WHERE [Name] = 'Ashley Bond'
            UPDATE [PolicyContacts] SET [AssignedToId] = 8 WHERE [Name] = 'Christina Millard'
            UPDATE [PolicyContacts] SET [AssignedToId] = 20 WHERE [Name] = 'Christopher Guzallis'
            UPDATE [PolicyContacts] SET [AssignedToId] = 10 WHERE [Name] = 'Debbie Bulik '
            UPDATE [PolicyContacts] SET [AssignedToId] = 13 WHERE [Name] = 'Graham Stone'
            UPDATE [PolicyContacts] SET [AssignedToId] = 23 WHERE [Name] = 'Jason Bazemore'
            UPDATE [PolicyContacts] SET [AssignedToId] = 14 WHERE [Name] = 'Jason Dennis'
            UPDATE [PolicyContacts] SET [AssignedToId] = 6 WHERE [Name] = 'Jeff Russell'
            UPDATE [PolicyContacts] SET [AssignedToId] = 28 WHERE [Name] = 'Joe Ciccone'
            UPDATE [PolicyContacts] SET [AssignedToId] = 25 WHERE [Name] = 'John Bovill'
            UPDATE [PolicyContacts] SET [AssignedToId] = 9 WHERE [Name] = 'Jonathan Runyan'
            UPDATE [PolicyContacts] SET [AssignedToId] = 19 WHERE [Name] = 'Joseph Froistad'
            UPDATE [PolicyContacts] SET [AssignedToId] = 2 WHERE [Name] = 'Kurt Jensen'
            UPDATE [PolicyContacts] SET [AssignedToId] = 29 WHERE [Name] = 'Laura Brown'
            UPDATE [PolicyContacts] SET [AssignedToId] = 17 WHERE [Name] = 'Marchelle Thurman'
            UPDATE [PolicyContacts] SET [AssignedToId] = 22 WHERE [Name] = 'Marissa Niemoller'
            UPDATE [PolicyContacts] SET [AssignedToId] = 12 WHERE [Name] = 'Matthew Marko'
            UPDATE [PolicyContacts] SET [AssignedToId] = 18 WHERE [Name] = 'Sara Kenkel'
            UPDATE [PolicyContacts] SET [AssignedToId] = 21 WHERE [Name] = 'Sarah Lowman'
            UPDATE [PolicyContacts] SET [AssignedToId] = 5 WHERE [Name] = 'Taylor Redman'
            UPDATE [PolicyContacts] SET [AssignedToId] = 7 WHERE [Name] = 'Thomas Tegley III'
            UPDATE [PolicyContacts] SET [AssignedToId] = 16 WHERE [Name] = 'Vincent DiPolito'
            UPDATE [PolicyContacts] SET [AssignedToId] = 27 WHERE [Name] = 'Wade Ronspies'
            UPDATE [PolicyContacts] SET [AssignedToId] = 24 WHERE [Name] = 'Walter Buckley'

            SET IDENTITY_INSERT [dbo].[PolicyContacts] ON
            INSERT INTO [dbo].[PolicyContacts] ([Id], [Name], [Email], [Position], [AssignedToId]) VALUES(57, 'Jinky Garrido', 'jgarrido@rivpartners.com', 'Offshore SS', 26)
            SET IDENTITY_INSERT [dbo].[PolicyContacts] OFF
        ";
    }
}
