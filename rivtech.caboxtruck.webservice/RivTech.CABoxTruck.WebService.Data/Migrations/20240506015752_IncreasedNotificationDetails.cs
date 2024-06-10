using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class IncreasedNotificationDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NotificationDetails",
                table: "NotificationArchive",
                type: "varchar(6000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(2000)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NotificationDetails",
                table: "Notification",
                type: "varchar(6000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(2000)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NotificationDetails",
                table: "NotificationArchive",
                type: "varchar(2000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(6000)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NotificationDetails",
                table: "Notification",
                type: "varchar(2000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(6000)",
                oldNullable: true);
        }
    }
}
