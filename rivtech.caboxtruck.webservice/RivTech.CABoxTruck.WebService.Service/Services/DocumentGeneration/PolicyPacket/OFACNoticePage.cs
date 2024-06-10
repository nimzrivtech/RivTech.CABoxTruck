using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class OFACNoticePage : CommonValuesPage
    {
        public OFACNoticePage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Office of Foreign Assets Control.docx";
            PageOrder = 36;
            FormId = "OFACNotice";
        }
    }
}