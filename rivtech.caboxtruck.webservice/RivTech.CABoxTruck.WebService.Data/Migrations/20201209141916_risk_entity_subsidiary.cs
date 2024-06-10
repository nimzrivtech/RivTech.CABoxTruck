using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class risk_entity_subsidiary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearsInBusinessId",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "NAICSCodeID",
                table: "Entity");

            migrationBuilder.RenameColumn(
                name: "NewVenturePreviousExperienceId",
                table: "Risk",
                newName: "NewVenturePreviousExperienceId_old");

            migrationBuilder.AddColumn<short>(
                name: "NewVenturePreviousExperienceId",
                table: "Risk",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "NewVenturePreviousExperienceId_old",
                table: "Risk");

            migrationBuilder.AddColumn<short>(
                name: "AccountCategoryUWId",
                table: "Risk",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "YearsInBusiness",
                table: "Risk",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "YearsUnderCurrentManagement",
                table: "Risk",
                nullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "YearEstablished",
                table: "Entity",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescOperations",
                table: "Entity",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "NAICSCode",
                table: "Entity",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountCategoryUWId",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "YearsInBusiness",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "YearsUnderCurrentManagement",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "DescOperations",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "NAICSCode",
                table: "Entity");

            migrationBuilder.AlterColumn<Guid>(
                name: "NewVenturePreviousExperienceId",
                table: "Risk",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AddColumn<short>(
                name: "YearsInBusinessId",
                table: "Risk",
                type: "smallint",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "YearEstablished",
                table: "Entity",
                type: "int",
                nullable: true,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AddColumn<short>(
                name: "NAICSCodeID",
                table: "Entity",
                type: "smallint",
                nullable: true);
        }
    }
}
