using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class IndianaUninsuredMotoristCoveragePage : CommonValuesPage
    {
        public IndianaUninsuredMotoristCoveragePage(ReportData reportData) : base(reportData)
        {
            TemplateName = "IN/Indiana Uninsured Motorist Coverage.docx";
            PageOrder = 137;
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;

            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "IndianaUninsuredMotoristCoverage")?.Other;

            if (formOtherContent != null)
            {
                var formValues = JsonConvert.DeserializeObject<FormUMBIPDDTO>(formOtherContent);
                effectiveDate = formValues.UpdatedDate.ToString(DateFormat);
            }

            base.ReplaceFormValues("EffectiveDate", effectiveDate);
            FormValues.Add("EndorsementEffectiveDate", effectiveDate);
        }
    }
}