if not exists(select 1 from sys.objects where name like 'Billing_Invoice')
BEGIN
	CREATE EXTERNAL TABLE [Billing_Invoice](
		Id uniqueidentifier,RiskId uniqueidentifier,
		InvoiceNumber nvarchar(30),
		InvoiceDate datetime2,
		DueDate datetime2,
		PreviousBalance decimal(18,5),
		CurrentAmountInvoiced decimal(18,5),
		TotalAmountDue decimal(18,5),
		CreatedDate datetime2,
		CreatedById nvarchar(50),
		VoidDate datetime2,
		VoidedById nvarchar(50),
		IsFlatCancelInvoice bit)
	WITH (DATA_SOURCE = [Billing],SCHEMA_NAME = 'dbo',OBJECT_NAME = 'Invoice');
END;
GO

if not exists(select 1 from sys.objects where name like 'Billing_TransactionTaxBreakdown')
BEGIN
	CREATE EXTERNAL TABLE [dbo].Billing_TransactionTaxBreakdown(
		Id uniqueidentifier,
		AmountSubTypeId nvarchar(30),
		TaxableAmountSubTypeId nvarchar(30),
		TransactionId uniqueidentifier,
		Description nvarchar(1000),
		Amount decimal(18,5))
	WITH (DATA_SOURCE = [Billing],SCHEMA_NAME = 'dbo',OBJECT_NAME = 'TransactionTaxBreakdown');
END;
GO

if not exists(select 1 from sys.objects where name like 'Billing_Installment')
BEGIN
	CREATE EXTERNAL TABLE [Billing_Installment](
		Id uniqueidentifier NOT NULL,
		RiskId uniqueidentifier NOT NULL,
		InvoiceOnDate datetime2(7) NOT NULL,
		DueDate datetime2(7) NOT NULL,
		PercentageOfOutstanding decimal(18, 3) NOT NULL,
		IsInvoiced bit NOT NULL,
		CreatedDate datetime2(7) NOT NULL,
		CreatedById nvarchar(50),
		VoidDate datetime2(7),
		VoidedById nvarchar(50),
		InstallmentTypeId nvarchar(10),
		InvoiceId uniqueidentifier,
		IsSkipped bit NOT NULL
		)
	WITH (DATA_SOURCE = [Billing],SCHEMA_NAME = 'dbo',OBJECT_NAME = 'Installment');
END;
GO

if not exists(select 1 from sys.objects where name like 'Billing_InvoiceDetail')
BEGIN
	CREATE EXTERNAL TABLE [dbo].[Billing_InvoiceDetail](
		Id uniqueidentifier,
		InvoiceId uniqueidentifier,
		AmountSubTypeId nvarchar(30),
		InvoicedAmount decimal(18,5))
	WITH (DATA_SOURCE = [Billing],SCHEMA_NAME = 'dbo',OBJECT_NAME = 'InvoiceDetail');
END;
GO

if not exists(select 1 from sys.objects where name like 'Billing_Payment')
BEGIN
	CREATE EXTERNAL TABLE [Billing_Payment](
		Id uniqueidentifier,
		RiskId uniqueidentifier,
		Amount decimal(18,5),
		EffectiveDate datetime2,
		InstrumentId nvarchar(10),
		Reference nvarchar(50),
		Comment nvarchar(1000),
		CreatedDate datetime2,
		CreatedById nvarchar(50),
		VoidOfPaymentId uniqueidentifier,
		VoidedById nvarchar(50),
		VoidDate datetime2,
		PaymentTypeId nvarchar(10),
		PayeeInfoId uniqueidentifier,
		ClearDate datetime2,
		EscheatDate datetime2)
	WITH (DATA_SOURCE = [Billing],SCHEMA_NAME = 'dbo',OBJECT_NAME = 'Payment');
END;
GO

if not exists(select 1 from sys.objects where name like 'Billing_PaymentDetail')
BEGIN
	CREATE EXTERNAL TABLE [Billing_PaymentDetail](
		Id uniqueidentifier,
		PaymentId uniqueidentifier,
		Amount decimal(18,5),
		AmountTypeId nvarchar(10),
		AmountSubTypeId nvarchar(30))
	WITH (DATA_SOURCE = [Billing],SCHEMA_NAME = 'dbo',OBJECT_NAME = 'PaymentDetail');
END;
GO

if not exists(select 1 from sys.objects where name like 'Billing_PaymentDetailCommission')
BEGIN
	CREATE EXTERNAL TABLE [Billing_PaymentDetailCommission](
		Id uniqueidentifier,
		PaymentId uniqueidentifier,
		Amount decimal(18,5),
		AmountTypeId nvarchar(10),
		AmountSubTypeId nvarchar(30))
	WITH (DATA_SOURCE = [Billing],SCHEMA_NAME = 'dbo',OBJECT_NAME = 'PaymentDetailCommission');
END;
GO

if not exists(select 1 from sys.objects where name like 'Billing_AmountSubType')
BEGIN
    CREATE EXTERNAL TABLE [Billing_AmountSubType](
		Id nvarchar(30),
		Description nvarchar(100),
		IsActive bit,
		AmountTypeId nvarchar(10))
	WITH (DATA_SOURCE = [Billing],SCHEMA_NAME = 'dbo',OBJECT_NAME = 'AmountSubType');
END;
GO	  

if not exists(select 1 from sys.objects where name like 'Agency_ProgramStateAgency')
BEGIN
	CREATE EXTERNAL TABLE [Agency_ProgramStateAgency](
		[Id] [uniqueidentifier] NOT NULL,
		[ProgramId] [tinyint] NOT NULL,
		[AgencyId] [uniqueidentifier] NOT NULL,
		[CreatedBy] [bigint] NOT NULL,
		[CreatedDate] [datetime2](7) NOT NULL,
		[EffectiveDate] [datetime2](7) NULL,
		[ExpirationDate] [datetime2](7) NULL,
		[RemovedProcessDate] [datetime2](7) NULL,
		[RemovedBy] [bigint] NULL,
		[IsActive] [bit] NOT NULL,
		[StateCode] [varchar](2) NULL,
		)
	WITH (DATA_SOURCE = [Agency],SCHEMA_NAME = 'dbo',OBJECT_NAME = 'ProgramStateAgency');
END;
GO
if not exists(select 1 from sys.objects where name like 'Agency_Agency')
BEGIN
	CREATE EXTERNAL TABLE [dbo].[Agency_Agency](
	[Id] [uniqueidentifier] NOT NULL,
	[EntityId] [uniqueidentifier] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[EffectiveDate] [datetime2](7) NULL,
	[ExpirationDate] [datetime2](7) NULL,
	[RemovedProcessDate] [datetime2](7) NULL,
	[RemovedBy] [bigint] NULL,
	[AgencyManagementSystemId] [varchar](10) NULL,
	[AgencyNetworkId] [varchar](10) NULL,
	[AgencyStatusHistoryId] [uniqueidentifier] NULL,
	[CommisionTypeId] [varchar](1) NULL,
	[CommissionGroupId] [uniqueidentifier] NULL,
	[ContactName] [varchar](100) NULL,
	[EFTAccountNumber] [varchar](20) NULL,
	[EFTAccountTypeId] [varchar](10) NULL,
	[EFTEmail] [varchar](100) NULL,
	[EFTRoutingNumber] [varchar](20) NULL,
	[HasBindingAuthority] [bit] NOT NULL,
	[IsCommissionPaidToAgency] [bit] NOT NULL,
	[IsEFTAvailable] [bit] NOT NULL,
	[LicenseEffectiveDate] [datetime2](7) NULL,
	[LicenseExpirationDate] [datetime2](7) NULL,
	[RegionalSalesManagerId] [uniqueidentifier] NULL,
	[IsActive] [bit] NOT NULL,
	[AgencyCode] [varchar](20) NULL,
	[Is1099Required] [bit] NOT NULL,
	)WITH (DATA_SOURCE = [Agency],SCHEMA_NAME = 'dbo',OBJECT_NAME = 'Agency');
END;
GO
if not exists(select 1 from sys.objects where name like 'Agency_Entity')
BEGIN
	CREATE EXTERNAL TABLE [Agency_Entity](
	[Id] [uniqueidentifier] NOT NULL,
	[IsIndividual] [bit] NOT NULL,
	[FirstName] [varchar](40) NULL,
	[MiddleName] [varchar](40) NULL,
	[LastName] [varchar](40) NULL,
	[CompanyName] [varchar](100) NULL,
	[FullName] [varchar](130) NOT NULL,
	[HomePhone] [varchar](30) NULL,
	[MobilePhone] [varchar](30) NULL,
	[WorkPhone] [varchar](30) NULL,
	[WorkPhoneExtension] [varchar](30) NULL,
	[WorkFax] [varchar](30) NULL,
	[PersonalEmailAddress] [varchar](100) NULL,
	[WorkEmailAddress] [varchar](100) NULL,
	[BirthDate] [datetime2](7) NOT NULL,
	[Age] [int] NOT NULL,
	[DriverLicenseNumber] [varchar](130) NULL,
	[DriverLicenseState] [varchar](30) NULL,
	[DriverLicenseExpiration] [datetime2](7) NULL,
	[GenderID] [tinyint] NULL,
	[MaritalStatusID] [tinyint] NULL,
	[BusinessTypeID] [tinyint] NULL,
	[DBA] [varchar](100) NULL,
	[YearEstablished] [int] NULL,
	[FederalIDNumber] [varchar](20) NULL,
	[NAICSCodeID] [tinyint] NULL,
	[AdditionalNAICSCodeID] [tinyint] NULL,
	[ICCMCDocketNumber] [varchar](50) NULL,
	[USDOTNumber] [varchar](50) NULL,
	[PUCNumber] [varchar](50) NULL,
	[HasSubsidiaries] [bit] NOT NULL,
	[IsInternalUser] [bit] NOT NULL,
	[IsAgent] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[FEIN] [varchar](10) NULL,
	[IsRegionalSalesManager] [bit] NOT NULL,
	[SocialSecurityNumber] [varchar](9) NULL,
	[WorkPhone2] [varchar](30) NULL,
	[CarrierCode] [varchar](20) NULL,
	[ReportRecipient] [nvarchar](60) NULL,
	)WITH (DATA_SOURCE = [Agency],SCHEMA_NAME = 'dbo',OBJECT_NAME = 'Entity');
END;
GO