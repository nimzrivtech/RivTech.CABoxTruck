using System.Linq;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class PhysicalDamageLimitOfInsurancePage : CommonValuesPage
    {
        public PhysicalDamageLimitOfInsurancePage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Physical Damage Limit of Insurance Endorsement.docx";
            PageOrder = 25;
            FormId = "PhysicalDamageLimitOfInsurance";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;

            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "PhysicalDamageLimitOfInsurance")?.Other;

            if (formOtherContent != null)
            {
                var formValues = JsonConvert.DeserializeObject<FormBaseDTO>(formOtherContent);
                effectiveDate = formValues.UpdatedDate?.ToString(DateFormat);
            }

            FormValues.Add("FormEffectiveDate", effectiveDate);
            FormValues.Add("EndorsementEffectiveDate", effectiveDate);
        }
    }
}