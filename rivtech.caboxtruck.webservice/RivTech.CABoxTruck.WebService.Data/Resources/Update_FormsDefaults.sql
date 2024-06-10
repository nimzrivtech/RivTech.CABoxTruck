-- Reset
DELETE FROM [dbo].[RiskForm]

-- Set default checked
UPDATE Form SET IsDefault = 1
WHERE Id NOT IN 
('DriverExclusionUnder23','AdditionalInsuredBlanket', 'PhysicalDamageLimitOfInsurance', 'UIIAUpdated', 
'DesignatedInsured', 'TrailerInterchangeCoverage')

UPDATE Form SET IsDefault = 0
WHERE Id IN 
('DriverExclusionUnder23','AdditionalInsuredBlanket', 'PhysicalDamageLimitOfInsurance', 'UIIAUpdated', 
'DesignatedInsured', 'TrailerInterchangeCoverage')

UPDATE [dbo].[Form] SET [SortOrder] = 34 WHERE Id = 'OFACNotice'
UPDATE [dbo].[Form] SET [SortOrder] = 35 WHERE Id = 'TreasuryNotice'

-- Delete first forms to be id renamed
DELETE FROM [dbo].[Form] WHERE Id IN ('ManuscriptEndorsement', 'VehiclesDrivingBeyondDesignatedRadiusOfOperation', 'UnitedSpecialtyAutoPolicyCoverage')

INSERT INTO [dbo].[Form]([Id],[Name],[SortOrder],[IsActive],[FormNumber],[FileName],[IsSupplementalDocument],[CreatedBy],[CreatedDate],[IsMandatory],[IsDefault])
     VALUES ('TexasInsuranceAutoPolicyCoverage','Texas Insurance Auto Policy Coverage',5,1,'RPCA005','TexasInsuranceAutoPolicyCoverage.docx',0,0,GETDATE(),1,1)

INSERT INTO [dbo].[Form]([Id],[Name],[SortOrder],[IsActive],[FormNumber],[FileName],[IsSupplementalDocument],[CreatedBy],[CreatedDate],[IsMandatory],[IsDefault])
     VALUES ('ExclusionRadiusEndorsement','Exclusion Radius Endorsement',6,1,'RPCA006','ExclusionRadiusEndorsement.docx',0,0,GETDATE(),0,1)

INSERT INTO [dbo].[Form]([Id],[Name],[SortOrder],[IsActive],[FormNumber],[FileName],[IsSupplementalDocument],[CreatedBy],[CreatedDate],[IsMandatory],[IsDefault])
     VALUES ('GeneralLiabilityEndorsement','General Liability Endorsement',30,1,'RPCA030','GeneralLiabilityEndorsement.docx',0,0,GETDATE(),0,0)

INSERT INTO [dbo].[Form]([Id],[Name],[SortOrder],[IsActive],[FormNumber],[FileName],[IsSupplementalDocument],[CreatedBy],[CreatedDate],[IsMandatory],[IsDefault])
     VALUES ('MotorTruckCargoEndorsement','Motor Truck Cargo Endorsement',31,1,'RPCA031','MotorTruckCargoEndorsement.docx',0,0,GETDATE(),0,0)

INSERT INTO [dbo].[Form]([Id],[Name],[SortOrder],[IsActive],[FormNumber],[FileName],[IsSupplementalDocument],[CreatedBy],[CreatedDate],[IsMandatory],[IsDefault])
     VALUES ('ManuscriptEndorsementProRate','Manuscript Endorsement ProRate',32,1,'RPCAMANU','ManuscriptEndorsementProRate.docx',0,0,GETDATE(),0,0)

INSERT INTO [dbo].[Form]([Id],[Name],[SortOrder],[IsActive],[FormNumber],[FileName],[IsSupplementalDocument],[CreatedBy],[CreatedDate],[IsMandatory],[IsDefault])
     VALUES ('ManuscriptEndorsementFullEarn','Manuscript Endorsement Full Earn',33,1,'RPCAMANU','ManuscriptEndorsementFullEarn.docx',0,0,GETDATE(),0,0)



