using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class AdditionalInsuredBlanketPage : CommonValuesPage
    {
        public AdditionalInsuredBlanketPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Additional Insured Endorsement.docx";
            PageOrder = 24;
            FormId = "AdditionalInsuredBlanket";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;
            var additionalInsuredEntities = new List<Dictionary<string, string>>();
            if (ReportData.Risk.BlanketCoverage.IsAdditionalInsured.GetValueOrDefault() && !IsEndorsement)
            {
                additionalInsuredEntities.Add(new Dictionary<string, string> { { "Table_Names_Item", BlanketMessage } });
            }
            else
            {
                var additionalInsured = ReportData.Risk.AdditionalInterest.FirstOrDefault(ai => ai.Id == new Guid(ReportData.Id));
                if (additionalInsured != null)
                {
                    effectiveDate = additionalInsured.EffectiveDate?.ToString(DateFormat);
                    if (AdditionalInterestChanged(additionalInsured.Id)) effectiveDate = ReportData.Endorsement.EffectiveDate.ToString(DateFormat);

                    additionalInsuredEntities.Add(new Dictionary<string, string> { { "Table_Names_Item", additionalInsured.Name } });
                }
            }

            TableValues = new Dictionary<string, List<Dictionary<string, string>>>
            {
                {"Names_", additionalInsuredEntities}
            };

            FormValues.Add("FormEffectiveDate", effectiveDate);
            FormValues.Add("EndorsementEffectiveDate", effectiveDate);

        }
    }
}