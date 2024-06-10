using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class AddPendingCancellationNoticeDateAndCancellationReasonToRisk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CancellationReason",
                table: "Risk",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PendingCancellationNoticeDate",
                table: "Risk",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancellationReason",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "PendingCancellationNoticeDate",
                table: "Risk");
        }
    }
}
