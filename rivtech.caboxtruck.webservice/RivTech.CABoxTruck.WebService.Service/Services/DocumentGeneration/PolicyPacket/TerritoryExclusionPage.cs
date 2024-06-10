using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class TerritoryExclusionPage : CommonValuesPage
    {
        public TerritoryExclusionPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Territory Exclusion Endorsement.docx";
            PageOrder = 26;
            FormId = "TerritoryExclusion";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;
            var endorsementEffectiveDate = EndorsementEffectiveDate;
            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "TerritoryExclusion")?.Other;
            if (formOtherContent == null) return;

            var formInputs = JsonConvert.DeserializeObject<TerritoryExclusionDTO>(formOtherContent);

            if(ReportData.Endorsement != null || formInputs?.UpdatedDate != null)
                effectiveDate = formInputs.UpdatedDate?.ToString(DateFormat);

            base.ReplaceFormValues("EffectiveDate", effectiveDate);
            FormValues.Add("EndorsementEffectiveDate", effectiveDate);


            var states = GetStatesList(formInputs.States?.Split(","));

            if (states != null)
            {
                var stateEntities = new List<Dictionary<string, string>>();

                foreach (var state in states)
                {
                    stateEntities.Add(new Dictionary<string, string>
                {
                    { "Table_States_Item", state?.Replace("[E]", "") }
                });
                }

                TableValues = new Dictionary<string, List<Dictionary<string, string>>>
                {
                    {"States_", stateEntities}
                };
            }
        }

        private List<string> GetStatesList(IEnumerable<string> stateCodes)
        {
            return stateCodes?.Select(code =>
                    (string) ReportData.DataSets["States"].FirstOrDefault(r => r.StateCode == code)?.FullStateName)
                .OrderBy(s => s).ToList();
        }
    }

}