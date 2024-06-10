using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class BillingTransactionExternalTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(ScriptUtils.CreateTableWithAzureChecking(CREATE_ExternalTable_Billing_Transaction, "Billing_Transaction"));
            migrationBuilder.Sql(ScriptUtils.CreateTableWithAzureChecking(CREATE_ExternalTable_Billing_TransactionDetail, "Billing_TransactionDetail"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(DELETE_ExternalTable_Billing_Transaction);
            migrationBuilder.Sql(DELETE_ExternalTable_Billing_TransactionDetail);
        }

        #region Billing Transaction Table

        const string CREATE_ExternalTable_Billing_Transaction = @"
            CREATE EXTERNAL TABLE [dbo].[Billing_Transaction]
            (
                [Id] [uniqueidentifier] NOT NULL,
                [RiskId] [uniqueidentifier] NOT NULL,
                [TransactionTypeId] [nvarchar](10) NULL,
                [EffectiveDate] [datetime2](7) NOT NULL,
                [CreatedDate] [datetime2](7) NOT NULL,
                [CreatedById] [nvarchar](50) NULL,
                [InvoiceId] [uniqueidentifier] NULL
            )
            WITH (DATA_SOURCE = [Billing],SCHEMA_NAME = N'dbo',OBJECT_NAME = N'Transaction');
        ";

        const string DELETE_ExternalTable_Billing_Transaction = @"
            DROP EXTERNAL TABLE Billing_Transaction;
        ";

        #endregion

        #region Billing TransactionDetail table

        const string CREATE_ExternalTable_Billing_TransactionDetail = @"
            CREATE EXTERNAL TABLE [dbo].[Billing_TransactionDetail]
            (
                [Id] [uniqueidentifier] NOT NULL,
                [Amount] [decimal](18, 5) NOT NULL,
                [AmountSubTypeId] [nvarchar](30) NULL,
                [TransactionId] [uniqueidentifier] NOT NULL,
                [VoidDate] [datetime2](7) NULL,
                [VoidedById] [nvarchar](50) NULL,
                [VoidedTransactionDetailId] [uniqueidentifier] NULL,
                [InvoiceDate] [datetime2](7) NULL,
                [Description] [nvarchar](max) NULL,
                [AmountTypeId] [nvarchar](max) NULL
            )
            WITH (DATA_SOURCE = [Billing],SCHEMA_NAME = N'dbo',OBJECT_NAME = N'TransactionDetail')
        ";

        const string DELETE_ExternalTable_Billing_TransactionDetail = @"
            DROP EXTERNAL TABLE Billing_TransactionDetail;
        ";


        #endregion

    }
}
