using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Linq;
using RivTech.CABoxTruck.WebService.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class ExclusionRadiusEndorsementPage : CommonValuesPage
    {
        public ExclusionRadiusEndorsementPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Exclusion Radius Endorsement.docx";
            PageOrder = 8;
            FormId = "ExclusionRadiusEndorsement";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var formOtherContent = ReportData.Risk.RiskForms
                .FirstOrDefault(f => f.FormId == "ExclusionRadiusEndorsement")?.Other;

            if (formOtherContent == null) return;

            var formInputs = JsonConvert.DeserializeObject<ExclusionRadiusDTO>(formOtherContent);
            if (IsEndorsement) base.ReplaceFormValues("EffectiveDate", formInputs.UpdatedDate?.ToString(DateFormat));
            FormValues.Add("Radius", formInputs?.Radius.ToString("N0"));
        }
    }
}