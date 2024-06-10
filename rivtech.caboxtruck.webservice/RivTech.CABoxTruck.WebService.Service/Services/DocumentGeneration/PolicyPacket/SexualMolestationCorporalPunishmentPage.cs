using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class SexualMolestationCorporalPunishmentPage : CommonValuesPage
    {
        public SexualMolestationCorporalPunishmentPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Sexual Molestation Exclusion.docx";
            PageOrder = 13;
            FormId = "SexualMolestationCorporalPunishment";
        }
    }
}