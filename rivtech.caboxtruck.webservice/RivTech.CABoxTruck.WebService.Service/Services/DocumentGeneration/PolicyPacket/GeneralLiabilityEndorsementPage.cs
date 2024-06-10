using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class GeneralLiabilityEndorsementPage : CommonValuesPage
    {
        public GeneralLiabilityEndorsementPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "GL Liability Coverage form.docx";
            PageOrder = 31;
            FormId = "GeneralLiabilityEndorsement";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;
            var endorsementEffectiveDate = EndorsementEffectiveDate;
            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "GeneralLiabilityEndorsement")?.Other;

            if (formOtherContent == null) return;

            var formInputs = JsonConvert.DeserializeObject<GeneralLiabilityDTO>(formOtherContent);

            effectiveDate = formInputs.UpdatedDate?.ToString(DateFormat) ?? PolicyEffectiveDate;

            base.ReplaceFormValues("EffectiveDate", effectiveDate);
        }
    }
}