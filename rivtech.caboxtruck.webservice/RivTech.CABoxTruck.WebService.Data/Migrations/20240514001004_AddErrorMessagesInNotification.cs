using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddErrorMessagesInNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErrorMessages",
                table: "NotificationArchive",
                type: "varchar(6000)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessages",
                table: "Notification",
                type: "varchar(6000)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorMessages",
                table: "NotificationArchive");

            migrationBuilder.DropColumn(
                name: "ErrorMessages",
                table: "Notification");
        }
    }
}
