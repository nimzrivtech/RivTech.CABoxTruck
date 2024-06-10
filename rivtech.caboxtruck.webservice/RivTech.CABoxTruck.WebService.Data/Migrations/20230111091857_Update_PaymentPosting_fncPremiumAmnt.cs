using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Update_PaymentPosting_fncPremiumAmnt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(Up_FncPremiumAmnt);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(Down_FncPremiumAmnt);
        }
        const string Up_FncPremiumAmnt = @"
         ALTER FUNCTION [dbo].[fnPremiumAmount](
            @PaymentId uniqueidentifier,
            @PaymentTypeId INT
    
        )
        RETURNS DEC(10,2)
        AS 
        BEGIN
        DECLARE @PremiumAmount AS DEC (10,2),
        @PayId AS uniqueidentifier = @PaymentId

		        IF @PaymentTypeId != 3
			        SET @PremiumAmount =ISNULL((select SUM(Amount) from Billing_PaymentDetail
			        where PaymentId=@PayId and (AmountTypeId = 'P' or AmountTypeId = 'C')),0)
		        ELSE
			        SET @PremiumAmount=ISNULL((select SUM(Amount) from Billing_PaymentDetail
			        where PaymentId=@PayId and AmountTypeId = 'P' ),0)

        Return @PremiumAmount
    
        END ";


        const string Down_FncPremiumAmnt = @"
        CREATE FUNCTION fnPremiumAmount(
            @PaymentId uniqueidentifier,
            @PaymentTypeId INT
    
        )
        RETURNS DEC(10,2)
        AS 
        BEGIN
        DECLARE @PremiumAmount AS DEC (10,2),
        @PayId AS uniqueidentifier = @PaymentId

		        IF @PaymentTypeId != 3
			        SET @PremiumAmount =ISNULL((select SUM(Amount) from Billing_PaymentDetail
			        where PaymentId=@PayId and AmountTypeId = 'P' or AmountTypeId = 'C'),0)
		        ELSE
			        SET @PremiumAmount=ISNULL((select SUM(Amount) from Billing_PaymentDetail
			        where PaymentId=@PayId and AmountTypeId = 'P' ),0)

        Return @PremiumAmount
    
        END ";

        
    }
}
