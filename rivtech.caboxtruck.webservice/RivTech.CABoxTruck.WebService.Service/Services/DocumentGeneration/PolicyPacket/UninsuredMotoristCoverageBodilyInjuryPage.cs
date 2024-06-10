using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class UninsuredMotoristCoverageBodilyInjuryPage : CommonValuesPage
    {
        public UninsuredMotoristCoverageBodilyInjuryPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "California Uninsured Motorist Coverage_Bodily Injury.docx";
            PageOrder = 100;
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;

            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "UninsuredMotoristCoverageBodilyInjury")?.Other;

            if (formOtherContent != null)
            {
                var formValues = JsonConvert.DeserializeObject<FormUMBIDTO>(formOtherContent);
                effectiveDate = formValues.UpdatedDate.ToString(DateFormat);
            }

            base.ReplaceFormValues("EffectiveDate", effectiveDate);
            FormValues.Add("EndorsementEffectiveDate", effectiveDate);

            //var uninsuredLimit = GetLimitsText(Coverage.UMBILimitId, "UMBILimits");
            //if (!string.IsNullOrEmpty(uninsuredLimit) && uninsuredLimit != "NONE")
            //{
            //    FormValues.Add("Limit", uninsuredLimit);
            //}

            //var (person, accident) = GetLimitsSplitText(Coverage.UMBILimitId, "UMBILimits");
            //if (!string.IsNullOrEmpty(person))
            //{
            //    FormValues.Add("LimitPerson", person);
            //}
            //if (!string.IsNullOrEmpty(accident))
            //{
            //    FormValues.Add("LimitAccident", accident);
            //}
        }
    }
}