using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class CoveredAutoDesignationSymbolPage : CommonValuesPage
    {
        public CoveredAutoDesignationSymbolPage(ReportData reportData) : base(reportData)
        {
            FormId = "CoveredAutoDesignationSymbol";

            var dataSet = ReportData.DataSets["SymbolTenResponses"];
            int symbolTenResponseId = 0;
            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "CoveredAutoDesignationSymbol")?.Other;
            var formValues = JsonConvert.DeserializeObject<CoveredAutoDesigSymbolDTO>(formOtherContent);

            int.TryParse(formValues.CoveredAutoText, out symbolTenResponseId);
            var response = dataSet.FirstOrDefault(x => x.Id == symbolTenResponseId);

            if (response != null)
            {
                if (symbolTenResponseId == 6)
                    TemplateName = "Covered Auto Designation Symbol Others.docx";
                else
                    TemplateName = "Covered Auto Designation Symbol.docx";
            }
            
            PageOrder = 41;
        }

        protected override void SetFormValues()
        {
            bool hasOthers = TemplateName.Contains("Other");
            base.SetFormValues();

            if (hasOthers)
                TemplateName = TemplateName.Replace("Covered Auto Designation Symbol", "Covered Auto Designation Symbol Others");

            var effectiveDate = PolicyEffectiveDate;

            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "CoveredAutoDesignationSymbol")?.Other;

            if (formOtherContent != null)
            {
                var formValues = JsonConvert.DeserializeObject<CoveredAutoDesigSymbolDTO>(formOtherContent);
                var dataSet = ReportData.DataSets["SymbolTenResponses"];
                int symbolTenResponseId = 0;

                int.TryParse(formValues.CoveredAutoText, out symbolTenResponseId);
                var response = dataSet.FirstOrDefault(x => x.Id == symbolTenResponseId);

                if(response != null)
                {
                    if(symbolTenResponseId == 6)
                        FormValues.Add("SymbolTenResponse", formValues.TextForEndorsement);
                    else
                        FormValues.Add("SymbolTenResponse", response.Description);
                }

                if (ReportData.Endorsement != null || formValues?.UpdatedDate != null)
                    effectiveDate = formValues.UpdatedDate?.ToString(DateFormat);
            }

            base.ReplaceFormValues("EffectiveDate", effectiveDate);
            FormValues.Add("EndorsementEffectiveDate", effectiveDate);
        }
    }
}