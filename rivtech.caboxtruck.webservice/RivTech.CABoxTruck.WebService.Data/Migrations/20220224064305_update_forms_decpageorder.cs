using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class update_forms_decpageorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_UPDATE);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

        const string SQL_UPDATE = @"
            Update [Form] Set SortOrder = 4, DecPageOrder = 4 where Id = 'InstallmentSchedule'
            Update [Form] Set SortOrder = 5, DecPageOrder = 5 where Id = 'HowToReportAClaim'
            Update [Form] Set SortOrder = 6, DecPageOrder = NULL where Id = 'TexasInsuranceAutoPolicyCoverage'
            Update [Form] Set SortOrder = 7, DecPageOrder = NULL where Id = 'ExclusionRadiusEndorsement'
            Update [Form] Set SortOrder = 8, DecPageOrder = 8 where Id = 'OccupantHazardExclusionEndorsement'
            Update [Form] Set SortOrder = 9, DecPageOrder = 9 where Id = 'UnlistedDriverExclusion'
            Update [Form] Set SortOrder = 10, DecPageOrder = 10 where Id = 'MCS90'
            Update [Form] Set SortOrder = 11, DecPageOrder = 11 where Id = 'OperatingUnderThe'
            Update [Form] Set SortOrder = 12, DecPageOrder = 12 where Id = 'SexualMolestationCorporalPunishment'
            Update [Form] Set SortOrder = 13, DecPageOrder = 13 where Id = 'ElectronicDataAndCyberRiskExclusion'
            Update [Form] Set SortOrder = 14, DecPageOrder = 14 where Id = 'NuclearDamageExclusion'
            Update [Form] Set SortOrder = 15, DecPageOrder = 15 where Id = 'WarAndTerrorismExclusion'
            Update [Form] Set SortOrder = 16, DecPageOrder = 16 where Id = 'HiredPhysicalDamage'
            Update [Form] Set SortOrder = 17, DecPageOrder = 17 where Id = 'DefenseOutsideTheLimitsEndorsement'
            Update [Form] Set SortOrder = 18, DecPageOrder = 18 where Id = 'PrimaryAndNoncontributory'
            Update [Form] Set SortOrder = 19, DecPageOrder = 19 where Id = 'WaiverOfTransferOfRights'
            GO
        ";
    }
}
