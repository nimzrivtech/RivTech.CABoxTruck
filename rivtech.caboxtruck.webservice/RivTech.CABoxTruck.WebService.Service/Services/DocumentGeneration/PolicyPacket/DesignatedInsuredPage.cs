using System;
using System.Collections.Generic;
using System.Linq;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class DesignatedInsuredPage : CommonValuesPage
    {
        public DesignatedInsuredPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Designated Insured.docx";
            PageOrder = 28;
            FormId = "DesignatedInsured";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;
            var designatedInsuredEntities = new List<Dictionary<string, string>>();

            var designatedInsured = ReportData.Risk.AdditionalInterest.FirstOrDefault(ai => ai.Id == new Guid(ReportData.Id));
            if (designatedInsured != null)
            {
                effectiveDate = designatedInsured.EffectiveDate?.ToString(DateFormat);
                if (AdditionalInterestChanged(designatedInsured.Id)) effectiveDate = ReportData.Endorsement.EffectiveDate.ToString(DateFormat);
                
                designatedInsuredEntities.Add(new Dictionary<string, string> {{"Table_Names_Item", designatedInsured?.Name}});
            }

            TableValues = new Dictionary<string, List<Dictionary<string, string>>>
            {
                {"Names_", designatedInsuredEntities}
            };

            FormValues.Add("FormEffectiveDate", effectiveDate);
            FormValues.Add("EndorsementEffectiveDate", effectiveDate);
        }
    }
}