using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFormVersionSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_INSERT);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_DELETE);
        }

        const string SQL_INSERT = @"
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'AdditionalInsuredBlanket',2,'RPCAAC023_1','Additional Insured Endorsement','FormsV2/Additional Insured Endorsement.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'BusinessAutoPolicyDeclarations',2,'RPCAAC003_1','Business Auto Policy Declarations','FormsV2/Business Auto Policy Declarations.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'CourierCoverageEndorsement',2,'RPCAAC033_1','Courier Coverage Endorsement','FormsV2/BT Courier Coverage Endorsement.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'CoveredAutoDesignationSymbol',2,'RPCAAC053_1','Covered Auto Designation Symbol','FormsV2/Covered Auto Designation Symbol.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'DefenseOutsideTheLimitsEndorsement',2,'RPCAAC016_1','Defense Outside the Limits Endorsement','FormsV2/Defense Outside the Limits Endorsement.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'DesignatedInsured',2,'RPCAAC028_1','Designated Insured','FormsV2/Designated Insured.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'DriverExclusion',2,'RPCAAC020_1','Driver Exclusion Endorsement','FormsV2/Driver Exclusion.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'DriverExclusionUnder23',2,'RPCAAC021_1','Driver Exclusion Under 23','FormsV2/Driver Exclusion under 23.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'DriverRequirements',2,'RPCAAC022_1','Driver Requirements Endorsement','FormsV2/Driver Requirements Endorsement.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'ElectronicDataAndCyberRiskExclusion',2,'RPCAAC012_1','Electronic Data and Cyber Risk Exclusion','FormsV2/Electronic Data and Cyber Risk Exclusion.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'ExclusionRadiusEndorsement',2,'RPCAAC006_1','Exclusion Radius Endorsement','FormsV2/Exclusion Radius Endorsement.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'GeneralLiabilityEndorsement',2,'RPCAAC030_1','General Liability Endorsement','FormsV2/GL Liability Coverage form.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'HiredPhysicalDamage',2,'RPCAAC015_1','Hired Physical Damage Endorsement','FormsV2/Hired Physical Damage Form.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'HowToReportAClaim',2,'RPCAAC004_1','How to Report a Claim','FormsV2/How to Report a Claim_Applied.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'InstallmentSchedule',2,'RPCAAC019_1','Installment Schedule','FormsV2/Installment Schedule.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'InsuranceBinder',2,'RPCAAC002_1','Insurance Binder','FormsV2/Insurance Binder New System.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'LossPayeeEndorsement',2,'RPCAAC032_1','Loss Payee Endorsement','FormsV2/Loss Payee Endorsement.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'MCS90',2,'FORM MCS-90','MCS90','FormsV2/MCS90.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'MotorTruckCargoEndorsement',2,'RPCAAC031_1','Motor Truck Cargo Endorsement','FormsV2/Motor Truck Cargo Endorsement.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'NuclearDamageExclusion',2,'RPCAAC013_1','Nuclear Damage Exclusion','FormsV2/Nuclear Damage Exclusion.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'OccupantHazardExclusionEndorsement',2,'RPCAAC007_1','Occupant Hazard Exclusion Endorsement','FormsV2/OccupantHazardExclusion.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'OFACNotice',2,'RIV-CA-1120-OFAC-N','OFAC Notice','FormsV2/Office of Foreign Assets Control.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'OperatingUnderThe',2,'RPCAAC010_1','Operating Under the Influence of Drugs and Alcohol Endorsement','FormsV2/Operating Under the influence Drugs and Alcohol Endorsement.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'PhysicalDamageCoverage',2,'RPCAAC036_1','Physical Damage Coverage Form','FormsV2/Physical Damage Coverage Form.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'PhysicalDamageLimitOfInsurance',2,'RPCAAC024_1','Physical Damage Limit of Insurance Endorsement','FormsV2/Physical Damage Limit of Insurance Endorsement.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'PolicyHolderPrivacyStatement',2,'RPCAAC001_1','Policy Holder Privacy Statement','FormsV2/Policy Holder Privacy Statement.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'PollutionCoverage',2,'CA 99 48 10 13','Pollution Coverage','FormsV2/Pollution Coverage.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'PrimaryAndNoncontributory',2,'RPCAAC017_1','Primary and Noncontributory','FormsV2/Primary And Noncontributory.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'RefrigerationCargoEndorsement',2,'RPCAAC034_1','Refrigeration Cargo Endorsement','FormsV2/BT Refrigeration Cargo Endorsement.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'SexualMolestationCorporalPunishment',2,'RPCAAC011_1','Sexual Molestation Exclusion','FormsV2/Sexual Molestation Exclusion.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'SurplusLinesNotice',2,'(State #)','Surplus Lines Notice','FormsV2/State','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'TerritoryExclusion',2,'RPCAAC025_1','Territory Exclusion Endorsement','FormsV2/Territory Exclusion Endorsement.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'TexasInsuranceAutoPolicyCoverage',2,'RPCAAC005_02','Texas Insurance Auto Policy Coverage','FormsV2/Texas Insurance Auto Policy Coverage.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'TreasuryNotice',2,'RIV-CA-1120-TES-N','Trade or Economic Sanctions','FormsV2/Trade or Economic Sanctions.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'TrailerInterchangeCoverageAL',2,'RPCAAC029_1','Trailer Interchange Coverage','FormsV2/Trailer Interchange Coverage AL.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'UIIAUpdated',2,'RPCAAC027_1','UIIA Endorsement','FormsV2/UIIA Endorsement.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'UnlistedDriverExclusion',2,'RPCAAC008_1','Unlisted Driver Exclusion','FormsV2/UnlistedDriverExclusion.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'WaiverOfTransferOfRights',2,'RPCAAC018_1','Waiver of Transfer of Right','FormsV2/Waiver of Transfer of Right.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'WarAndTerrorismExclusion',2,'RPCAAC014_1','War and Terrorism Exclusion','FormsV2/War And Terrorism Exclusion.docx','12-20-2023')
            INSERT INTO FormVersion (Id,FormId,Version,FormNumber,FileName,FilePath,EffectiveDate) VALUES(NEWID(),'ManuscriptEndorsementFullEarn',2,'RPCAACMANUFE_1','Manuscript Endorsement Full Earn','FormsV2/Manuscript Endorsement Full Earn.docx','12-20-2023')
            GO
        ";

        const string SQL_DELETE = @"
            DELETE FROM FormVersion
            GO
        ";
    }
}
