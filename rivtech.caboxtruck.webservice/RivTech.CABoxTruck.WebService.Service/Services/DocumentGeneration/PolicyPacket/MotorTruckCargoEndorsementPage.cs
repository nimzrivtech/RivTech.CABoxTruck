using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class MotorTruckCargoEndorsementPage : CommonValuesPage
    {
        public MotorTruckCargoEndorsementPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Motor Truck Cargo Endorsement.docx";
            PageOrder = 32;
            FormId = "MotorTruckCargoEndorsement";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;
            var endorsementEffectiveDate = EndorsementEffectiveDate;
            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "MotorTruckCargoEndorsement")?.Other;

            if (formOtherContent == null) return;

            var formInputs = JsonConvert.DeserializeObject<FormBaseDTO>(formOtherContent);

            effectiveDate = formInputs.UpdatedDate?.ToString(DateFormat);

            base.ReplaceFormValues("EffectiveDate", effectiveDate);
        }
    }
}