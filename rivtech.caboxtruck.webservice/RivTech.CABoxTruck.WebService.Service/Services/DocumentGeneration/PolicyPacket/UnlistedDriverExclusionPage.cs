using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class UnlistedDriverExclusionPage : CommonValuesPage
    {
        public UnlistedDriverExclusionPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "UnlistedDriverExclusion.docx";
            PageOrder = 10;
            FormId = "UnlistedDriverExclusion";
        }
    }
}