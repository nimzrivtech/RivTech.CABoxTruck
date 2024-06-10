using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class NuclearDamageExclusionPage : CommonValuesPage
    {
        public NuclearDamageExclusionPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Nuclear Damage Exclusion.docx";
            PageOrder = 15;
            FormId = "NuclearDamageExclusion";
        }
    }
}