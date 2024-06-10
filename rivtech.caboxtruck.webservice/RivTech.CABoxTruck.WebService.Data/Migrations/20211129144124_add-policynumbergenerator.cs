using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class addpolicynumbergenerator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(sp_GeneratePolicyNumber);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("if OBJECT_ID('GetNextPolicyNumber') is not null drop procedure [GetNextPolicyNumber]");
        }

        const string sp_GeneratePolicyNumber = @"
            ALTER SEQUENCE nextPolicyNumber RESTART WITH 9001
            GO
            CREATE OR ALTER PROCEDURE [dbo].[GetNextPolicyNumber] AS 
            BEGIN
	            SET NOCOUNT ON;
	            declare @remainder int
	            declare @nextValue int
	            select @nextValue = next value for nextPolicyNumber
	            if (@nextValue % 1000 = 0) begin
	            select @nextValue = next value for nextPolicyNumber
	            end

	            declare @firstPart int
	            declare @secondPart int
	            set @firstPart = (@nextvalue / 1000)
	            set @secondPart = @nextValue % 1000;

	            SELECT CASE WHEN @firstPart > 9 THEN NCHAR(@firstPart + 55) ELSE CONVERT(VARCHAR(10), @firstPart) END + FORMAT(@secondPart,'00#')
            END
        ";
    }
}
