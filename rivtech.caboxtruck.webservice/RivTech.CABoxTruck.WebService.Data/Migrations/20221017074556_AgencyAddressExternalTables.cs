using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class AgencyAddressExternalTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(ScriptUtils.CreateTableWithAzureChecking(CREATE_ExternalTable_Agency_Address, "Agency_Address"));
            migrationBuilder.Sql(ScriptUtils.CreateTableWithAzureChecking(CREATE_ExternalTable_Agency_Entity_Address, "Agency_EntityAddress"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(DELETE_ExternalTable_Agency_Address);
            migrationBuilder.Sql(DELETE_ExternalTable_Agency_Entity_Address);
        }

        #region Agency Address Table

        const string CREATE_ExternalTable_Agency_Address = @"
            CREATE EXTERNAL TABLE [dbo].[Agency_Address]
            (
	            [Id] [uniqueidentifier] NOT NULL,
	            [StreetAddress1] [varchar](200) NULL,
	            [StreetAddress2] [varchar](200) NULL,
	            [ZipCodeExt] [varchar](10) NULL,
	            [Longitude] [decimal](11, 8) NOT NULL,
	            [Latitude] [decimal](10, 8) NOT NULL,
	            [IsGarageIndoor] [bit] NOT NULL,
	            [IsGarageOutdoor] [bit] NOT NULL,
	            [IsGarageFenced] [bit] NOT NULL,
	            [IsGarageLighted] [bit] NOT NULL,
	            [IsGarageWithSecurityGuard] [bit] NOT NULL,
	            [CreatedBy] [bigint] NOT NULL,
	            [CreatedDate] [datetime2](7) NOT NULL,
	            [EffectiveDate] [datetime2](7) NULL,
	            [ExpirationDate] [datetime2](7) NULL,
	            [RemovedProcessDate] [datetime2](7) NULL,
	            [RemovedBy] [bigint] NULL,
	            [City] [varchar](50) NULL,
	            [CountryCode] [varchar](50) NULL,
	            [County] [varchar](50) NULL,
	            [StateCode] [varchar](3) NULL,
	            [ZipCode] [varchar](15) NULL,
	            [IsActive] [bit] NOT NULL
            )
            WITH (DATA_SOURCE = [Agency],SCHEMA_NAME = N'dbo',OBJECT_NAME = N'Address')
        ";

        const string DELETE_ExternalTable_Agency_Address = @"
            DROP EXTERNAL TABLE Agency_Address;
        ";

        #endregion

        #region Agency Entity Address Table

        const string CREATE_ExternalTable_Agency_Entity_Address = @"
            CREATE EXTERNAL TABLE [dbo].[Agency_EntityAddress]
			(
				[Id] [uniqueidentifier] NOT NULL,
				[EntityId] [uniqueidentifier] NOT NULL,
				[AddressId] [uniqueidentifier] NOT NULL,
				[AddressTypeId] [tinyint] NOT NULL,
				[CreatedBy] [bigint] NOT NULL,
				[CreatedDate] [datetime2](7) NOT NULL,
				[EffectiveDate] [datetime2](7) NULL,
				[ExpirationDate] [datetime2](7) NULL,
				[RemovedProcessDate] [datetime2](7) NULL,
				[RemovedBy] [bigint] NULL,
				[IsActive] [bit] NOT NULL
			)
			WITH (DATA_SOURCE = [Agency],SCHEMA_NAME = N'dbo',OBJECT_NAME = N'EntityAddress')	
        ";

        const string DELETE_ExternalTable_Agency_Entity_Address = @"
            DROP EXTERNAL TABLE Agency_EntityAddress;
        ";

        #endregion
    }
}
