using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class TexasChangesPage : CommonValuesPage
    {
        public TexasChangesPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "TX/Texas Changes.docx";
            PageOrder = 103;
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;

            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "TexasChanges")?.Other;

            if (formOtherContent != null)
            {
                var formValues = JsonConvert.DeserializeObject<FormBaseDTO>(formOtherContent);
                effectiveDate = formValues.UpdatedDate?.ToString(DateFormat);
            }

            base.ReplaceFormValues("EffectiveDate", effectiveDate);
        }

        // Migration
        // Insert into[Form] values('TexasChanges','Texas Changes', 42, 1, 'CA 01 96 11 20', 'TX/Texas Changes.docx', 42, 0, 0, GETDATE(),1, 1)
    }
}