using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class TexasSplitUninsuredandUnderinsuredMotoristCoveragePage : CommonValuesPage
    {
        public TexasSplitUninsuredandUnderinsuredMotoristCoveragePage(ReportData reportData) : base(reportData)
        {
            TemplateName = "TX/Texas Split Uninsured and Underinsured Motorist Coverage Form.docx";
            PageOrder = 106;
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;

            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "TexasSplitUninsuredandUnderinsuredMotoristCoverage")?.Other;

            if (formOtherContent != null)
            {
                var formValues = JsonConvert.DeserializeObject<FormTXUMUIMDTO>(formOtherContent);
                effectiveDate = formValues.UpdatedDate?.ToString(DateFormat);
            }

            base.ReplaceFormValues("EffectiveDate", effectiveDate);
            FormValues.Add("EndorsementEffectiveDate", effectiveDate);
        }

        // Migration
        // Insert into[Form] values('TexasSplitUninsuredandUnderinsuredMotoristCoverage','Texas Split Uninsured and Underinsured Motorist Coverage Form', 45, 1, 'CA 31 25 02 01', 'TX/Texas Split Uninsured and Underinsured Motorist Coverage Form.docx', 45, 0, 0, GETDATE(),0, 1)
    }
}