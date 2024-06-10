using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class TreasuryNoticePage : CommonValuesPage
    {
        public TreasuryNoticePage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Trade or Economic Sanctions.docx";
            PageOrder = 37;
            FormId = "TreasuryNotice";
        }
    }
}