using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class CourierCoverageEndorsementPage : CommonValuesPage
    {
        public CourierCoverageEndorsementPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "BT Courier Coverage Endorsement.docx";
            PageOrder = 39;
            FormId = "CourierCoverageEndorsement";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var formOtherContent = ReportData.Risk.RiskForms
                .FirstOrDefault(f => f.FormId == "CourierCoverageEndorsement")?.Other;

            if (formOtherContent == null) return;

            var formInputs = JsonConvert.DeserializeObject<FormBaseDTO>(formOtherContent);
            base.ReplaceFormValues("EffectiveDate", formInputs.UpdatedDate?.ToString(DateFormat));
        }
    }
}