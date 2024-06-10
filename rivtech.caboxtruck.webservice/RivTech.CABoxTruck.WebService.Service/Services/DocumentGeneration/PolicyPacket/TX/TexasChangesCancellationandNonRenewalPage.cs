using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class TexasChangesCancellationandNonRenewalPage : CommonValuesPage
    {
        public TexasChangesCancellationandNonRenewalPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "TX/Texas Changes - Cancellation and Non-Renewal.docx";
            PageOrder = 102;
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;

            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "TexasChangesCancellationandNonRenewal")?.Other;

            if (formOtherContent != null)
            {
                var formValues = JsonConvert.DeserializeObject<FormBaseDTO>(formOtherContent);
                effectiveDate = formValues.UpdatedDate?.ToString(DateFormat);
            }

            base.ReplaceFormValues("EffectiveDate", effectiveDate);
        }

        // Migration
        // Insert into[Form] values('TexasChangesCancellationandNonRenewal','Texas Changes - Cancellation and Non-Renewal', 41, 1, 'CA 02 43 11 13', 'TX/Texas Changes - Cancellation and Non-Renewal.docx', 41, 0, 0, GETDATE(),1, 1)
    }
}