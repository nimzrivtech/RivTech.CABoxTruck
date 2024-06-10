using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class entitysubsidiarycontact_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfVehicles",
                table: "EntitySubsidiary");

            migrationBuilder.DropColumn(
                name: "SizeOfCompany",
                table: "EntitySubsidiary");

            migrationBuilder.AlterColumn<short>(
                name: "AdditionalInterestTypeId",
                table: "RiskAdditionalInterest",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AddColumn<bool>(
                name: "IsOthersAllowedOperate",
                table: "EntitySubsidiary",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOthersAllowedPrevOperate",
                table: "EntitySubsidiary",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubsidiaryCompany",
                table: "EntitySubsidiary",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NameYearOperations",
                table: "EntitySubsidiary",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<short>(
                name: "NumVechSizeComp",
                table: "EntitySubsidiary",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AlterColumn<short>(
                name: "ContactTypeId",
                table: "EntityContact",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<short>(
                name: "CompanyPositionId",
                table: "EntityContact",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "EntityContact",
                type: "varchar(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneExtension",
                table: "EntityContact",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOthersAllowedOperate",
                table: "EntitySubsidiary");

            migrationBuilder.DropColumn(
                name: "IsOthersAllowedPrevOperate",
                table: "EntitySubsidiary");

            migrationBuilder.DropColumn(
                name: "IsSubsidiaryCompany",
                table: "EntitySubsidiary");

            migrationBuilder.DropColumn(
                name: "NameYearOperations",
                table: "EntitySubsidiary");

            migrationBuilder.DropColumn(
                name: "NumVechSizeComp",
                table: "EntitySubsidiary");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "EntityContact");

            migrationBuilder.DropColumn(
                name: "PhoneExtension",
                table: "EntityContact");

            migrationBuilder.AlterColumn<byte>(
                name: "AdditionalInterestTypeId",
                table: "RiskAdditionalInterest",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(short));

            migrationBuilder.AddColumn<short>(
                name: "NumberOfVehicles",
                table: "EntitySubsidiary",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "SizeOfCompany",
                table: "EntitySubsidiary",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "ContactTypeId",
                table: "EntityContact",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<byte>(
                name: "CompanyPositionId",
                table: "EntityContact",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(short),
                oldNullable: true);
        }
    }
}
