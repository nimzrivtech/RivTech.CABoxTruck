using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class add_external_prebindpayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(ADD_EXTERNAL_PREBINDPAYMENT);
			migrationBuilder.Sql(CREATE_SP_GETPREBINDPAYMENTS);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

		const string ADD_EXTERNAL_PREBINDPAYMENT = @"
			declare @isAzure bit = IIF(ServerProperty('Edition') = 'SQL Azure', 1, 0)
			if (not exists(select 1 from sys.objects where name like 'Billing_PreBindPayment') and @isAzure = 1)
			BEGIN
				declare @script nvarchar(max) = '
					CREATE EXTERNAL TABLE [Billing_PreBindPayment](
						[Id] [uniqueidentifier] NOT NULL,
						[InsuredName] [varchar](100) NULL,
						[AccessCode] [varchar](100) NULL,
						[PayeeName] [varchar](100) NULL,
						[PayeeEmail] [varchar](100) NULL,
						[PaymentDateTime] [datetime2](7) NOT NULL,
						[PaymentMethod] [varchar](100) NULL,
						[PaymentAmount] [decimal](18, 5) NOT NULL,
						[ConfirmationCode] [nvarchar](max) NULL)
					WITH (DATA_SOURCE = [Billing],SCHEMA_NAME = ''dbo'',OBJECT_NAME = ''PreBindPayment'');
				'
				exec sp_executesql @script
				print 'Billing_PreBindPayment external table created...'
			END
			else
			begin
				print 'Failed to create external table Billing_PreBindPayment.'
			end
			GO
        ";

		const string CREATE_SP_GETPREBINDPAYMENTS = @"
            SET ANSI_NULLS ON
			GO
			SET QUOTED_IDENTIFIER ON
			GO

			CREATE OR ALTER PROCEDURE [dbo].[GeneratePreBindPayments]

					@fromDate datetime = NULL,
					@toDate datetime = NULL,
					@sql nvarchar(max) = null
				AS
				BEGIN

				SET NOCOUNT ON;

				set @sql = 
				'
					select 
						Id,
						InsuredName,
						AccessCode,
						PayeeName,
						PayeeEmail,
						PaymentDateTime,
						PaymentMethod,
						PaymentAmount,
						ConfirmationCode
					from
						[Billing_PreBindPayment]
					where PaymentDateTime >= @fromDate and PaymentDateTime <= @toDate
				'
				
				SET @sql = REPLACE(@sql, '@fromDate', ISNULL('''' +  CONVERT(VARCHAR(20), @fromDate) + '''', 'NULL'));
				SET @sql = REPLACE(@sql, '@toDate', ISNULL('''' +  CONVERT(VARCHAR(20), @toDate) + '''', 'NULL'));

				print(@sql)
					EXEC sp_executesql @sql
				END
        ";
	}
}
