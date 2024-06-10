using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
   
    public class GeorgiaUMCoverageReducedbyatFaultLiabilityLimitsPage : CommonValuesPage
    {
        public GeorgiaUMCoverageReducedbyatFaultLiabilityLimitsPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "GA/Georgia Uninsured Motorist Coverage Reduced by atFault Liability Limits.docx";
            PageOrder = 113;
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;

            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "GeorgiaUMCoverageReducedbyatFaultLiabilityLimits")?.Other;

            if (formOtherContent != null)
            {
                var formValues = JsonConvert.DeserializeObject<FormBaseDTO>(formOtherContent);
                effectiveDate = formValues.UpdatedDate?.ToString(DateFormat);
            }

            base.ReplaceFormValues("EffectiveDate", effectiveDate);
        }

    }
}
