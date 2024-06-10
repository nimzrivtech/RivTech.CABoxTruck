using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class PolicyHolderPrivacyStatementPage : CommonValuesPage
    {
        public PolicyHolderPrivacyStatementPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Policy Holder Privacy Statement.docx";
            PageOrder = 1;
            FormId = "PolicyHolderPrivacyStatement";
        }
    }
}