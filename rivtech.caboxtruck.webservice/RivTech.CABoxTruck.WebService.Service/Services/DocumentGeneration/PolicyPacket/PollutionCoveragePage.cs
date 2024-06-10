using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class PollutionCoveragePage : CommonValuesPage
    {
        public PollutionCoveragePage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Pollution Coverage.docx";
            PageOrder = 43;
            FormId = "PollutionCoverage";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;

            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "PollutionCoverage")?.Other;

            if (formOtherContent != null)
            {
                var formValues = JsonConvert.DeserializeObject<PollutionCoverageDTO>(formOtherContent);
                effectiveDate = formValues.UpdatedDate?.ToString(DateFormat);
            }
            base.ReplaceFormValues("EffectiveDate2", effectiveDate);
            base.ReplaceFormValues("EffectiveDate", effectiveDate);
        }
    }
}