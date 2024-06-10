using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class TexasInsuranceAutoPolicyCoveragePage : CommonValuesPage
    {
        public TexasInsuranceAutoPolicyCoveragePage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Texas Insurance Auto Policy Coverage.docx";
            PageOrder = 7;
            FormId = "TexasInsuranceAutoPolicyCoverage";
        }
    }
}