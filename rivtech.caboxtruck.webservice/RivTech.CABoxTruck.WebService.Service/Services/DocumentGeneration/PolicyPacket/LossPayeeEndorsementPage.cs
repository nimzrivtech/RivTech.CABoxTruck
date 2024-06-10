using System;
using System.Linq;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class LossPayeeEndorsementPage : CommonValuesPage
    {
        public LossPayeeEndorsementPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Loss Payee Endorsement.docx";
            PageOrder = 38;
            FormId = "LossPayeeEndorsement";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;

            var lossPayee = ReportData.Risk.AdditionalInterest.FirstOrDefault(ai => ai.Id == new Guid(ReportData.Id));
            if (lossPayee != null)
            {
                effectiveDate = lossPayee.EffectiveDate?.ToString(DateFormat);

                FormValues.Add("LossPayeeName", lossPayee.Name);
                FormValues.Add("LossPayeeAddress", $"{lossPayee.Address}, {lossPayee.City}, {lossPayee.State} {lossPayee.ZipCode}");

                var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.Form.Id == "LossPayeeEndorsement")?.Other;
                if (formOtherContent != null)
                {
                    var formInputs = JsonConvert.DeserializeObject<FormLossPayeeDTO>(formOtherContent);
                    FormValues.Add("DescriptionOfAutos", formInputs.DescriptionOfAutos);
                }
            }

            FormValues.Add("FormEffectiveDate", effectiveDate);
            FormValues.Add("EndorsementEffectiveDate", effectiveDate);
        }
    }
}