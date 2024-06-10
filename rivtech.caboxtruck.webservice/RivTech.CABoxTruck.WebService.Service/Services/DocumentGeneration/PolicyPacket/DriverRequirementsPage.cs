using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class DriverRequirementsPage : CommonValuesPage
    {
        public DriverRequirementsPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Driver Requirements Endorsement.docx";
            PageOrder = 23;
            FormId = "DriverRequirements";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "DriverRequirements")?.Other;

            if (formOtherContent == null) return;

            var formInputs = JsonConvert.DeserializeObject<DriverRequirementDTO>(formOtherContent);
            base.ReplaceFormValues("EffectiveDate", formInputs.UpdatedDate?.ToString(DateFormat) ?? PolicyEffectiveDate);
            FormValues.Add("Requirements", formInputs?.Requirement);
        }
    }
}