using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class remove_required_fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "YearsWithAgency",
                table: "Risk",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<short>(
                name: "YearsInBusinessId",
                table: "Risk",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<string>(
                name: "SubmissionNumber",
                table: "Risk",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<short>(
                name: "RiskStateId",
                table: "Risk",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<short>(
                name: "RenewalCount",
                table: "Risk",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<string>(
                name: "QuoteNumber",
                table: "Risk",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<short>(
                name: "ProgramID",
                table: "Risk",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<short>(
                name: "PrimaryCityZipCodeOfOperationsId",
                table: "Risk",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "MainUseId",
                table: "Risk",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<short>(
                name: "AverageMilesPerVehicleId",
                table: "Risk",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<short>(
                name: "AccountCategoryId",
                table: "Risk",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<string>(
                name: "TypeOfBusiness",
                table: "EntitySubsidiary",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Relationship",
                table: "EntitySubsidiary",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<short>(
                name: "NumVechSizeComp",
                table: "EntitySubsidiary",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<string>(
                name: "NameYearOperations",
                table: "EntitySubsidiary",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "EntitySubsidiary",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "EntitySubsidiary",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "ContactTypeId",
                table: "EntityContact",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<short>(
                name: "AddressTypeId",
                table: "EntityAddress",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<short>(
                name: "NAICSCodeID",
                table: "Entity",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "MaritalStatusID",
                table: "Entity",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "GenderID",
                table: "Entity",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Entity",
                type: "varchar(130)",
                maxLength: 130,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(130)",
                oldMaxLength: 130);

            migrationBuilder.AlterColumn<short>(
                name: "BusinessTypeID",
                table: "Entity",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "Entity",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "Entity",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<short>(
                name: "AdditionalNAICSCodeID",
                table: "Entity",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StreetAddress1",
                table: "Address",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Address",
                type: "decimal(11, 8)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(11, 8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Address",
                type: "decimal(10, 8)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10, 8)");

            migrationBuilder.AlterColumn<int>(
                name: "CityZipCodeId",
                table: "Address",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "YearsWithAgency",
                table: "Risk",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "YearsInBusinessId",
                table: "Risk",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SubmissionNumber",
                table: "Risk",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RiskStateId",
                table: "Risk",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "RenewalCount",
                table: "Risk",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QuoteNumber",
                table: "Risk",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "ProgramID",
                table: "Risk",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "PrimaryCityZipCodeOfOperationsId",
                table: "Risk",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "MainUseId",
                table: "Risk",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "AverageMilesPerVehicleId",
                table: "Risk",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "AccountCategoryId",
                table: "Risk",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TypeOfBusiness",
                table: "EntitySubsidiary",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Relationship",
                table: "EntitySubsidiary",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "NumVechSizeComp",
                table: "EntitySubsidiary",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameYearOperations",
                table: "EntitySubsidiary",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "EntitySubsidiary",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "EntitySubsidiary",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<short>(
                name: "ContactTypeId",
                table: "EntityContact",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "AddressTypeId",
                table: "EntityAddress",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "NAICSCodeID",
                table: "Entity",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "MaritalStatusID",
                table: "Entity",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "GenderID",
                table: "Entity",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Entity",
                type: "varchar(130)",
                maxLength: 130,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(130)",
                oldMaxLength: 130,
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "BusinessTypeID",
                table: "Entity",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "Entity",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "Entity",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "AdditionalNAICSCodeID",
                table: "Entity",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StreetAddress1",
                table: "Address",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Address",
                type: "decimal(11, 8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(11, 8)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Address",
                type: "decimal(10, 8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10, 8)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CityZipCodeId",
                table: "Address",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
