using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class remove_HiredDate_StateId_Outstate_duplicate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HiredDate",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "OutStateDriver",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Driver");

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "HiredDate",
                table: "Driver",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OutStateDriver",
                table: "Driver",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "StateId",
                table: "Driver",
                type: "smallint",
                nullable: true);
        }
    }
}
