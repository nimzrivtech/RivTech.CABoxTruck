using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePolicyContactsAddNewColumnIsAssignedTo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedToId",
                table: "PolicyContacts");

            migrationBuilder.AddColumn<bool>(
                name: "IsAssignedTo",
                table: "PolicyContacts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql(UPDATE_SCRIPT);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAssignedTo",
                table: "PolicyContacts");

            migrationBuilder.AddColumn<int>(
                name: "AssignedToId",
                table: "PolicyContacts",
                type: "int",
                nullable: true);
        }

        const string UPDATE_SCRIPT = @"
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Ashley Bond'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Christina Millard'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Christopher Guzallis'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Graham Stone'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Jason Bazemore'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Jason Dennis'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Jeff Russell'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Joe Ciccone'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'John Bovill'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Jonathan Runyan'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Joseph Froistad'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Kurt Jensen'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Laura Brown'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Marchelle Thurman'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Marissa Niemoller'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Matthew Marko'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Sara Kenkel'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Sarah Lowman'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Taylor Redman'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Thomas Tegley III'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Vincent DiPolito'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Wade Ronspies'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Walter Buckley'
            UPDATE [PolicyContacts] SET [IsAssignedTo] = 1 WHERE [Name] = 'Jinky Garrido'
        ";
    }
}
