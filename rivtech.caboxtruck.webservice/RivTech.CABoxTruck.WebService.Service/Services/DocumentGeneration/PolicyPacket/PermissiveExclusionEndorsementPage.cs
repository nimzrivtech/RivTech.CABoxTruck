using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class PermissiveExclusionEndorsementPage : CommonValuesPage
    {
        public PermissiveExclusionEndorsementPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Permissive Use Exclusion Endorsement.docx";
            PageOrder = 44;
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "PermissiveExclusionEndorsement")?.Other;

            if (formOtherContent != null)
            {
                var formValues = JsonConvert.DeserializeObject<FormBaseDTO>(formOtherContent);
                base.ReplaceFormValues("EffectiveDate", formValues.UpdatedDate?.ToString(DateFormat));
            }
        }
    }
}