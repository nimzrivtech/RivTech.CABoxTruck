using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class PennsylvaniaUMCoverageLimitsNSPage : CommonValuesPage
    {
        public PennsylvaniaUMCoverageLimitsNSPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "PA/Pennsylvania Uninsured Motorist Coverage - Nonstacked.docx";
            PageOrder = 121;
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;

            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "PennsylvaniaUMCoverageLimitsNS")?.Other;

            if (formOtherContent != null)
            {
                var formValues = JsonConvert.DeserializeObject<FormUMBIDTO>(formOtherContent);
                effectiveDate = formValues.UpdatedDate.ToString(DateFormat);
            }

            base.ReplaceFormValues("EffectiveDate", effectiveDate);
            FormValues.Add("EndorsementEffectiveDate", effectiveDate);
        }
    }
}