using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class HowToReportAClaimPage : CommonValuesPage
    {
        public HowToReportAClaimPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "How to Report a Claim_Applied.docx";
            PageOrder = 6;
            FormId = "HowToReportAClaim";
        }
    }
}