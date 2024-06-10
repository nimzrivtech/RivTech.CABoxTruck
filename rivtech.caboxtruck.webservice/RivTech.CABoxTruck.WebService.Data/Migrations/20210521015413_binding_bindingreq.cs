using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class binding_bindingreq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Binding_PricingOptions_PricingOptionId",
                table: "Binding");

            migrationBuilder.DropIndex(
                name: "IX_Binding_PricingOptionId",
                table: "Binding");

            migrationBuilder.DropColumn(
               name: "PricingOptionId",
               table: "Binding");

            migrationBuilder.DropTable(
                name: "PricingOptions");

            migrationBuilder.CreateTable(
               name: "PaymentTypes",
               columns: table => new
               {
                   Id = table.Column<short>(type: "smallint", nullable: false),
                   Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                   IsActive = table.Column<bool>(type: "bit", nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_PaymentTypes", x => x.Id);
               });

            migrationBuilder.AddColumn<short>(
                name: "PaymentTypesId",
                table: "Binding",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Binding_PaymentTypesId",
                table: "Binding",
                column: "PaymentTypesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Binding_PaymentTypes_PaymentTypesId",
                table: "Binding",
                column: "PaymentTypesId",
                principalTable: "PaymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddColumn<string>(
                name: "Describe",
                table: "BindingRequirements",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Binding_PaymentTypes_PaymentTypesId",
                table: "Binding");

            migrationBuilder.DropTable(
                name: "PaymentTypes");

            migrationBuilder.DropColumn(
                name: "Describe",
                table: "BindingRequirements");

            migrationBuilder.RenameColumn(
                name: "PaymentTypesId",
                table: "Binding",
                newName: "PricingOptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Binding_PaymentTypesId",
                table: "Binding",
                newName: "IX_Binding_PricingOptionId");

            migrationBuilder.CreateTable(
                name: "PricingOptions",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricingOptions", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Binding_PricingOptions_PricingOptionId",
                table: "Binding",
                column: "PricingOptionId",
                principalTable: "PricingOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
