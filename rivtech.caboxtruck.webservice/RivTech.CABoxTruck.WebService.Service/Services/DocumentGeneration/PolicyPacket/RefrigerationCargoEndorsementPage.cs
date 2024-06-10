using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class RefrigerationCargoEndorsementPage : CommonValuesPage
    {
        public RefrigerationCargoEndorsementPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "BT Refrigeration Cargo Endorsement.docx";
            PageOrder = 40;
            FormId = "RefrigerationCargoEndorsement";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;
            var endorsementEffectiveDate = EndorsementEffectiveDate;
            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "RefrigerationCargoEndorsement")?.Other;

            if (formOtherContent == null) return;

            var formInputs = JsonConvert.DeserializeObject<FormBaseDTO>(formOtherContent);

            effectiveDate = formInputs.UpdatedDate?.ToString(DateFormat);

            base.ReplaceFormValues("EffectiveDate", effectiveDate);
        }
    }
}