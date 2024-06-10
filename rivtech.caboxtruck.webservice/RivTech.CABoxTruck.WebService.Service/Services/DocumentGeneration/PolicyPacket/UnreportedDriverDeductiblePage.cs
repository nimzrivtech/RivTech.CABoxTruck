using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class UnreportedDriverDeductiblePage : CommonValuesPage
    {
        public UnreportedDriverDeductiblePage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Unreported Driver Deductible.docx";
            PageOrder = 46;
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "UnreportedDriverDeductible")?.Other;

            if (formOtherContent != null)
            {
                var formValues = JsonConvert.DeserializeObject<FormBaseDTO>(formOtherContent);
                base.ReplaceFormValues("EffectiveDate", formValues.UpdatedDate?.ToString(DateFormat));
                base.ReplaceFormValues("EffectiveDate2", formValues.UpdatedDate?.ToString(DateFormat));
            }
        }
    }
}