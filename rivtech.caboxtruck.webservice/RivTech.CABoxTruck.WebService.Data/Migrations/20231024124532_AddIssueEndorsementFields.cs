using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIssueEndorsementFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveDate",
                table: "EndorsementProcessQueue",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndorsementText",
                table: "EndorsementProcessQueue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PolicyChanges",
                table: "EndorsementProcessQueue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RiskDetailId",
                table: "EndorsementProcessQueue",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EffectiveDate",
                table: "EndorsementProcessQueue");

            migrationBuilder.DropColumn(
                name: "EndorsementText",
                table: "EndorsementProcessQueue");

            migrationBuilder.DropColumn(
                name: "PolicyChanges",
                table: "EndorsementProcessQueue");

            migrationBuilder.DropColumn(
                name: "RiskDetailId",
                table: "EndorsementProcessQueue");
        }
    }
}
