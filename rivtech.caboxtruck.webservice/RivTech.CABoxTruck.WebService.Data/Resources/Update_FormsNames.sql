--New Forms
INSERT INTO [dbo].[Form]([Id],[Name],[SortOrder],[IsActive],[FormNumber],[FileName],[DecPageOrder],[IsSupplementalDocument],[CreatedBy],[CreatedDate],[IsMandatory])
     VALUES ('TrailerInterchangeCoverage','Trailer Interchange Coverage',32,1,'RPCA029','Trailer Interchange Coverage.docx',32,0,0,GETDATE(),0)
INSERT INTO [dbo].[Form]([Id],[Name],[SortOrder],[IsActive],[FormNumber],[FileName],[DecPageOrder],[IsSupplementalDocument],[CreatedBy],[CreatedDate],[IsMandatory])
     VALUES ('ManuscriptEndorsement','Manuscript Endorsement',33,1,'RPCAMANU','Manuscript Endorsement.docx',33,0,0,GETDATE(),0)

--Update to Names
UPDATE [dbo].[Form] SET [Name] = 'How to Report a Claim to NARS' WHERE Id = 'HowToReportAClaim'
UPDATE [dbo].[Form] SET [Name] = 'Operating Under the Influence of Drugs and Alcohol Endorsement' WHERE Id = 'OperatingUnderThe'
UPDATE [dbo].[Form] SET [Name] = 'Hired Physical Damage Endorsement' WHERE Id = 'HiredPhysicalDamage'
UPDATE [dbo].[Form] SET [Name] = 'Primary and Noncontributory' WHERE Id = 'PrimaryAndNoncontributory'
UPDATE [dbo].[Form] SET [Name] = 'Driver Exclusion Endorsement' WHERE Id = 'DriverExclusion'
UPDATE [dbo].[Form] SET [Name] = 'Driver Requirements Endorsement' WHERE Id = 'DriverRequirements'
UPDATE [dbo].[Form] SET [Name] = 'Additional Insured Endorsement' WHERE Id = 'AdditionalInsuredBlanket'
UPDATE [dbo].[Form] SET [Name] = 'Physical Damage Limit of Insurance Endorsement' WHERE Id = 'PhysicalDamageLimitOfInsurance'
UPDATE [dbo].[Form] SET [Name] = 'Territory Exclusion Endorsement' WHERE Id = 'TerritoryExclusion'
UPDATE [dbo].[Form] SET [Name] = 'UIIA Endorsement' WHERE Id = 'UIIAUpdated'