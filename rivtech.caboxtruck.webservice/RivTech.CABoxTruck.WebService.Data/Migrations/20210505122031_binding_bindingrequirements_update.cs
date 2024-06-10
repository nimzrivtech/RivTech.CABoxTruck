using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class binding_bindingrequirements_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"select * into TempBinding from Binding
                                   select * into TempBindingRequirements from BindingRequirements");

            migrationBuilder.Sql(@"truncate table  Binding");
            migrationBuilder.Sql(@"truncate table  BindingRequirements");

            //Binding

            migrationBuilder.AddColumn<Guid>(
                name: "RiskId",
                table: "Binding",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.DropForeignKey(
                name: "FK_Binding_RiskDetail_RiskDetailId",
                table: "Binding");

            migrationBuilder.DropIndex(
                name: "IX_Binding_RiskDetailId",
                table: "Binding");

            migrationBuilder.DropColumn(
                name: "RiskDetailId",
                table: "Binding");

            migrationBuilder.CreateIndex(
                name: "IX_Binding_RisKId",
                table: "Binding",
                column: "RiskId",
                unique: true,
                filter: "[RiskId] IS NOT NULL");


            //BindingRequirements
            migrationBuilder.AddColumn<Guid>(
                name: "RiskId",
                table: "BindingRequirements",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.DropForeignKey(
                name: "FK_BindingRequirements_RiskDetail_RiskDetailId",
                table: "BindingRequirements");

            migrationBuilder.DropIndex(
                name: "IX_BindingRequirements_RiskDetailId",
                table: "BindingRequirements");

            migrationBuilder.DropColumn(
                name: "RiskDetailId",
                table: "BindingRequirements");

            migrationBuilder.CreateIndex(
            name: "IX_BindingRequirements_RiskId",
            table: "BindingRequirements",
            columns: new[] { "RiskId", "Id" },
            unique: true,
            filter: "[RiskId] IS NOT NULL");






            migrationBuilder.AddForeignKey(
               name: "FK_Binding_Risk_RiskId",
               table: "Binding",
               column: "RiskId",
               principalTable: "Risk",
               principalColumn: "Id",
               onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BindingRequirements_Risk_RiskId",
                table: "BindingRequirements",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);


            migrationBuilder.Sql(@"insert into Binding(Id, BindOptionId, PricingOptionId, CreditOfficeId, SurplusLIneNum, RiskId, CreatedBy, CreatedDate, UpdatedDate)
                                   select tb.Id, tb.BindOptionId, tb.PricingOptionId, tb.CreditOfficeId, tb.SurplusLIneNum, RiskDetail.RiskId, tb.CreatedBy, tb.CreatedDate, tb.UpdatedDate from TempBinding tb
                                   inner join RiskDetail on RiskDetail.Id = tb.RiskDetailId");


            migrationBuilder.Sql(@"insert into BindingRequirements(Id, SubjectivitiesId, IsPreBind, BindStatusId, Comments, RelevantDocumentDesc, RiskId, CreatedBy, CreatedDate, UpdatedDate, IsActive, DeletedDate)
                                   select tbr.Id, tbr.SubjectivitiesId, tbr.IsPreBind, tbr.BindStatusId, tbr.Comments,tbr. RelevantDocumentDesc, RiskDetail.RiskId, tbr.CreatedBy, tbr.CreatedDate, tbr.UpdatedDate, tbr.IsActive, tbr.DeletedDate from TempBindingRequirements tbr
                                   inner join RiskDetail on RiskDetail.Id = tbr.RiskDetailId");


            migrationBuilder.DropTable(name: "TempBinding");
            migrationBuilder.DropTable(name: "TempBindingRequirements");

        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Binding_Risk_RiskId",
                table: "Binding");

            migrationBuilder.DropForeignKey(
                name: "FK_BindingRequirements_Risk_RiskId",
                table: "BindingRequirements");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "BindingRequirements",
                newName: "RiskDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_BindingRequirements_RiskId",
                table: "BindingRequirements",
                newName: "IX_BindingRequirements_RiskDetailId");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "Binding",
                newName: "RiskDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_Binding_RiskId",
                table: "Binding",
                newName: "IX_Binding_RiskDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Binding_RiskDetail_RiskDetailId",
                table: "Binding",
                column: "RiskDetailId",
                principalTable: "RiskDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BindingRequirements_RiskDetail_RiskDetailId",
                table: "BindingRequirements",
                column: "RiskDetailId",
                principalTable: "RiskDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
