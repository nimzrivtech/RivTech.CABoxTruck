using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class PennsylvaniaUIMCoverageLimitsNSPage : CommonValuesPage
    {
        public PennsylvaniaUIMCoverageLimitsNSPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "PA/Pennsylvania Underinsured Motorist Coverage - Nonstacked.docx";
            PageOrder = 122;
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;

            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "PennsylvaniaUIMCoverageLimitsNS")?.Other;

            if (formOtherContent != null)
            {
                var formValues = JsonConvert.DeserializeObject<FormUIMDTO>(formOtherContent);
                effectiveDate = formValues.UpdatedDate?.ToString(DateFormat);
            }

            base.ReplaceFormValues("EffectiveDate", effectiveDate);
            FormValues.Add("EndorsementEffectiveDate", effectiveDate);
        }
    }
}