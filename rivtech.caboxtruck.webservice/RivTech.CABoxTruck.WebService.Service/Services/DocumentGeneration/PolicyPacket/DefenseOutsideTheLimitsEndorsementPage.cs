using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class DefenseOutsideTheLimitsEndorsementPage : CommonValuesPage
    {
        public DefenseOutsideTheLimitsEndorsementPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Defense Outside the Limits Endorsement.docx";
            PageOrder = 18;
            FormId = "DefenseOutsideTheLimitsEndorsement";
        }
    }
}