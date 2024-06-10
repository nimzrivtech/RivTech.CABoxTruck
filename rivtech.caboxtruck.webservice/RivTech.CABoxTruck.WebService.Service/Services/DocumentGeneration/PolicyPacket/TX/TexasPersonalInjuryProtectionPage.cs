using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class TexasPersonalInjuryProtectionPage : CommonValuesPage
    {
        public TexasPersonalInjuryProtectionPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "TX/Texas Personal Injury Protection Endorsement.docx";
            PageOrder = 104;
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;

            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "TexasPersonalInjuryProtection")?.Other;

            if (formOtherContent != null)
            {
                var formValues = JsonConvert.DeserializeObject<FormPIPDTO>(formOtherContent);
                effectiveDate = formValues.UpdatedDate.ToString(DateFormat);
            }

            base.ReplaceFormValues("EffectiveDate", effectiveDate);
            FormValues.Add("EndorsementEffectiveDate", effectiveDate);
        }

        // Migration
        // Insert into[Form] values('TexasPersonalInjuryProtection','Texas Personal Injury Protection Endorsement', 43, 1, 'CA 22 64 10 13', 'TX/Texas Personal Injury Protection Endorsement.docx', 43, 0, 0, GETDATE(),0, 1)
    }
}