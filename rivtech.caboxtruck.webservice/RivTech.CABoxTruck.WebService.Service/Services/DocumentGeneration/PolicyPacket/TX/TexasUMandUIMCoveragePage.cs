using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class TexasUMandUIMCoveragePage : CommonValuesPage
    {
        public TexasUMandUIMCoveragePage(ReportData reportData) : base(reportData)
        {
            TemplateName = "TX/Texas UM and UIM Coverage.docx";
            PageOrder = 105;
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;

            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "TexasUMandUIMCoverage")?.Other;

            if (formOtherContent != null)
            {
                var formValues = JsonConvert.DeserializeObject<FormTXUMUIMDTO>(formOtherContent);
                effectiveDate = formValues.UpdatedDate?.ToString(DateFormat);
            }

            base.ReplaceFormValues("EffectiveDate", effectiveDate);
            FormValues.Add("EndorsementEffectiveDate", effectiveDate);
        }

        // Migration
        // Insert into[Form] values('TexasUMandUIMCoverage','Texas UM and UIM Coverage', 44, 1, 'CA 21 09 10 13', 'TX/Texas UM and UIM Coverage.docx', 44, 0, 0, GETDATE(),0, 1)
    }
}