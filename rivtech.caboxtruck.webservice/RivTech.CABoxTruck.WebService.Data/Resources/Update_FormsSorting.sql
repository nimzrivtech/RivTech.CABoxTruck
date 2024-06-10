-- update forms sorting order
UPDATE [dbo].[Form] SET [SortOrder] = 1, [DecPageOrder] = 1 WHERE [Id] = 'PolicyHolderPrivacyStatement'
UPDATE [dbo].[Form] SET [SortOrder] = 2, [DecPageOrder] = 2 WHERE [Id] = 'SurplusLinesNotice'
UPDATE [dbo].[Form] SET [SortOrder] = 3, [DecPageOrder] = 3 WHERE [Id] = 'InsuranceBinder'
UPDATE [dbo].[Form] SET [SortOrder] = 4, [DecPageOrder] = 4 WHERE [Id] = 'BusinessAutoPolicyDeclarations'
UPDATE [dbo].[Form] SET [SortOrder] = 5, [DecPageOrder] = 5 WHERE [Id] = 'UnitedSpecialtyAutoPolicyCoverage'
UPDATE [dbo].[Form] SET [SortOrder] = 6, [DecPageOrder] = 6 WHERE [Id] = 'VehiclesDrivingBeyondDesignatedRadiusOfOperation'
UPDATE [dbo].[Form] SET [SortOrder] = 7, [DecPageOrder] = 7 WHERE [Id] = 'OccupantHazardExclusionEndorsement'
UPDATE [dbo].[Form] SET [SortOrder] = 8, [DecPageOrder] = 8 WHERE [Id] = 'DefenseOutsideTheLimitsEndorsement'
UPDATE [dbo].[Form] SET [SortOrder] = 9, [DecPageOrder] = 9 WHERE [Id] = 'ElectronicDataAndCyberRiskExclusion'
UPDATE [dbo].[Form] SET [SortOrder] = 10, [DecPageOrder] = 10 WHERE [Id] = 'HowToReportAClaim'
UPDATE [dbo].[Form] SET [SortOrder] = 11, [DecPageOrder] = 11 WHERE [Id] = 'NuclearDamageExclusion'
UPDATE	[dbo].[Form] 
SET		[SortOrder] = 12, 
		[DecPageOrder] = 12,
		[Name] = 'Operating Under the Influence of Alcohol or Drugs or Other Intoxicants/Mind-Altering Substances'
WHERE [Id] = 'OperatingUnderThe'
UPDATE [dbo].[Form] SET [SortOrder] = 13, [DecPageOrder] = 13 WHERE [Id] = 'SexualMolestationCorporalPunishment'
UPDATE [dbo].[Form] SET [SortOrder] = 14, [DecPageOrder] = 14 WHERE [Id] = 'UnlistedDriverExclusion'
UPDATE [dbo].[Form] SET [SortOrder] = 15, [DecPageOrder] = 15 WHERE [Id] = 'WarAndTerrorismExclusion'
UPDATE [dbo].[Form] SET [SortOrder] = 16, [DecPageOrder] = 16 WHERE [Id] = 'OFACNotice'
UPDATE [dbo].[Form] SET [SortOrder] = 17, [DecPageOrder] = 17 WHERE [Id] = 'TreasuryNotice'
UPDATE [dbo].[Form] SET [SortOrder] = 18, [DecPageOrder] = 18 WHERE [Id] = 'AdditionalInsuredBlanket'
UPDATE [dbo].[Form] SET [SortOrder] = 19, [DecPageOrder] = 19 WHERE [Id] = 'HiredPhysicalDamage'
UPDATE [dbo].[Form] SET [SortOrder] = 20, [DecPageOrder] = 20 WHERE [Id] = 'InstallmentSchedule'
UPDATE [dbo].[Form] SET [SortOrder] = 21, [DecPageOrder] = 21 WHERE [Id] = 'MCS90'
UPDATE [dbo].[Form] SET [SortOrder] = 22, [DecPageOrder] = 22 WHERE [Id] = 'PhysicalDamageLimitOfInsurance'
UPDATE 	[dbo].[Form] 
SET 	[SortOrder] = 23, 
		[DecPageOrder] = 23,
		[Name] = 'Primary and Noncontributory - Other Insurance Condition'
WHERE 	[Id] = 'PrimaryAndNoncontributory'
UPDATE [dbo].[Form] SET [SortOrder] = 24, [DecPageOrder] = 24 WHERE [Id] = 'WaiverOfTransferOfRights'
UPDATE [dbo].[Form] SET [SortOrder] = 25, [DecPageOrder] = 25 WHERE [Id] = 'DesignatedInsured'
UPDATE [dbo].[Form] SET [SortOrder] = 26, [DecPageOrder] = 26 WHERE [Id] = 'DriverExclusion'
UPDATE [dbo].[Form] SET [SortOrder] = 27, [DecPageOrder] = 27 WHERE [Id] = 'DriverExclusionUnder23'
UPDATE [dbo].[Form] SET [SortOrder] = 28, [DecPageOrder] = 28 WHERE [Id] = 'DriverRequirements'
UPDATE [dbo].[Form] SET [SortOrder] = 29, [DecPageOrder] = 29 WHERE [Id] = 'TerritoryExclusion'
UPDATE [dbo].[Form] SET [SortOrder] = 30, [DecPageOrder] = 30 WHERE [Id] = 'UIIAUpdated'
UPDATE [dbo].[Form] SET [SortOrder] = 31, [DecPageOrder] = 31 WHERE [Id] = 'VehicleUsageExclusion'

-- update payment types
UPDATE [dbo].[PaymentTypes] SET [Description] = 'Direct Bill Full Pay' WHERE [Id] = 1
UPDATE [dbo].[PaymentTypes] SET [Description] = 'Direct Bill Installments' WHERE [Id] = 2
UPDATE [dbo].[PaymentTypes] SET [Description] = 'Premium Financed' WHERE [Id] = 3
