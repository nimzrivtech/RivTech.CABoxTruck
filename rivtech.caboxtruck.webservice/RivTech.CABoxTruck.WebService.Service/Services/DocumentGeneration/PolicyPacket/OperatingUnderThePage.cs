using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class OperatingUnderThePage : CommonValuesPage
    {
        public OperatingUnderThePage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Operating Under the influence Drugs and Alcohol Endorsement.docx";
            PageOrder = 12;
            FormId = "OperatingUnderThe";
        }
    }
}