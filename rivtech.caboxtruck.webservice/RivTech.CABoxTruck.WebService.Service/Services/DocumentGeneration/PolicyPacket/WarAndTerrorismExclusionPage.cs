using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class WarAndTerrorismExclusionPage : CommonValuesPage
    {
        public WarAndTerrorismExclusionPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "War And Terrorism Exclusion.docx";
            PageOrder = 16;
            FormId = "WarAndTerrorismExclusion";
        }
    }
}