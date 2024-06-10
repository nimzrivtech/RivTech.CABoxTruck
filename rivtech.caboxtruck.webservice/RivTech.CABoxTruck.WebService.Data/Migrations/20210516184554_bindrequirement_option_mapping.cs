using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class bindrequirement_option_mapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //BINDING TABLE
            migrationBuilder.AddColumn<short>(
                name: "CreditedOfficeId",
                table: "Binding",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Binding_CreditedOfficeId",
                table: "Binding",
                column: "CreditedOfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Binding_CreditedOffices_CreditedOfficeId",
                table: "Binding",
                column: "CreditedOfficeId",
                principalTable: "CreditedOffices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);


            //BindingRequirements TABLE

            migrationBuilder.DropForeignKey(
                name: "FK_BindingRequirements_Subjectivities_SubjectivitiesId",
                table: "BindingRequirements");

            migrationBuilder.DropIndex(
                name: "IX_BindingRequirements_SubjectivitiesId",
                table: "BindingRequirements");

            migrationBuilder.DropColumn(
               name: "SubjectivitiesId",
               table: "BindingRequirements");

            migrationBuilder.AddColumn<short>(
                name: "BindRequirementsOptionId",
                table: "BindingRequirements",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BindingRequirements_BindRequirementsOption_BindRequirementsOptionId",
                table: "BindingRequirements",
                column: "BindRequirementsOptionId",
                principalTable: "BindRequirementsOption",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

        //migrationBuilder.DropIndex(
        //        name: "IX_BindingRequirements_BindRequirementsOptionId",
        //        table: "BindingRequirements");

            migrationBuilder.CreateIndex(
                name: "IX_BindingRequirements_BindRequirementsOptionId",
                table: "BindingRequirements",
                column: "BindRequirementsOptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Binding_CreditedOffices_CreditedOfficeId",
                table: "Binding");

            migrationBuilder.DropForeignKey(
                name: "FK_BindingRequirements_BindRequirementsOption_BindRequirementsOptionId",
                table: "BindingRequirements");

            migrationBuilder.DropIndex(
                name: "IX_Binding_CreditedOfficeId",
                table: "Binding");

            migrationBuilder.DropColumn(
                name: "CreditedOfficeId",
                table: "Binding");

            migrationBuilder.RenameColumn(
                name: "BindRequirementsOptionId",
                table: "BindingRequirements",
                newName: "SubjectivitiesId");

            migrationBuilder.RenameIndex(
                name: "IX_BindingRequirements_BindRequirementsOptionId",
                table: "BindingRequirements",
                newName: "IX_BindingRequirements_SubjectivitiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_BindingRequirements_Subjectivities_SubjectivitiesId",
                table: "BindingRequirements",
                column: "SubjectivitiesId",
                principalTable: "Subjectivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
