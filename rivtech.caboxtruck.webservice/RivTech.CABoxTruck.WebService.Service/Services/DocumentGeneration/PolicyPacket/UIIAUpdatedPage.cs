using System.Linq;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class UIIAUpdatedPage : CommonValuesPage
    {
        public UIIAUpdatedPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "UIIA Endorsement.docx";
            PageOrder = 27;
            FormId = "UIIAUpdated";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "UIIAUpdated")?.Other;
            var autoBILimitId = Coverage.AutoBILimitId;
            var effectiveDate = PolicyEffectiveDate;
            
            if (formOtherContent != null)
            {
                var formValues = JsonConvert.DeserializeObject<FormUIIADTO>(formOtherContent);
                autoBILimitId = formValues.AutoBILimitId;
                effectiveDate = formValues.UpdatedDate.ToString(DateFormat);
            }

            FormValues.Add("ALLimit", GetLimitsText(autoBILimitId, "AutoBILimits")?.Replace("$", ""));
            FormValues.Add("FormEffectiveDate", effectiveDate);
            FormValues.Add("EndorsementEffectiveDate", effectiveDate);
        }
    }
}