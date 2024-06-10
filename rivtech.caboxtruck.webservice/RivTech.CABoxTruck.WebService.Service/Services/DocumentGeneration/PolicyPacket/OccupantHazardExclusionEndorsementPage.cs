using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class OccupantHazardExclusionEndorsementPage : CommonValuesPage
    {
        public OccupantHazardExclusionEndorsementPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "OccupantHazardExclusion.docx";
            PageOrder = 9;
            FormId = "OccupantHazardExclusionEndorsement";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "OccupantHazardExclusionEndorsement")?.Other;

            if (formOtherContent != null)
            {
                var formValues = JsonConvert.DeserializeObject<FormBaseDTO>(formOtherContent);
                base.ReplaceFormValues("EffectiveDate", formValues.UpdatedDate?.ToString(DateFormat));
            }
        }
    }
}