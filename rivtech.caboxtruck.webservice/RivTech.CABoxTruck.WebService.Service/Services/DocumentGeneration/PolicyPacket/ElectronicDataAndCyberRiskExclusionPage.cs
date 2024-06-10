using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class ElectronicDataAndCyberRiskExclusionPage : CommonValuesPage
    {
        public ElectronicDataAndCyberRiskExclusionPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Electronic Data and Cyber Risk Exclusion.docx";
            PageOrder = 14;
            FormId = "ElectronicDataAndCyberRiskExclusion";
        }
    }
}