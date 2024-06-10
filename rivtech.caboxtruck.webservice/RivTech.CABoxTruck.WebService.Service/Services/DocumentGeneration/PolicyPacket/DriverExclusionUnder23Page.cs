using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class DriverExclusionUnder23Page : CommonValuesPage
    {
        public DriverExclusionUnder23Page(ReportData reportData) : base(reportData)
        {
            TemplateName = "Driver Exclusion under 23.docx";
            PageOrder = 22;
            FormId = "DriverExclusionUnder23";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;
            var endorsementEffectiveDate = EndorsementEffectiveDate;
            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "DriverExclusionUnder23")?.Other;

            if (formOtherContent == null) return;

            var formInputs = JsonConvert.DeserializeObject<FormBaseDTO>(formOtherContent);

            effectiveDate = formInputs.UpdatedDate?.ToString(DateFormat);

            base.ReplaceFormValues("EffectiveDate", effectiveDate);
        }
    }
}