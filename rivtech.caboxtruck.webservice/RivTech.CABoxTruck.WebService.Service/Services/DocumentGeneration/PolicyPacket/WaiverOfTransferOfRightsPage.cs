using System;
using System.Collections.Generic;
using System.Linq;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class WaiverOfTransferOfRightsPage : CommonValuesPage
    {
        public WaiverOfTransferOfRightsPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Waiver of Transfer of Right.docx";
            PageOrder = 20;
            FormId = "WaiverOfTransferOfRights";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;
            var waiverEntities = new List<Dictionary<string, string>>();
            if (!IsEndorsement && ReportData.Risk.BlanketCoverage.IsWaiverOfSubrogation.GetValueOrDefault())
            {
                waiverEntities.Add(new Dictionary<string, string> {{"Table_Names_Item", BlanketMessage}});
            }
            else
            {
                var waiver = ReportData.Risk.AdditionalInterest.FirstOrDefault(ai => ai.Id == new Guid(ReportData.Id));
                if (waiver != null)
                {
                    effectiveDate = waiver.EffectiveDate?.ToString(DateFormat);
                    if (AdditionalInterestChanged(waiver.Id)) effectiveDate = ReportData.Endorsement.EffectiveDate.ToString(DateFormat);
                    waiverEntities.Add(new Dictionary<string, string> { { "Table_Names_Item", waiver.Name } });
                }
            }

            TableValues = new Dictionary<string, List<Dictionary<string, string>>>
            {
                {"Names_", waiverEntities}
            };

            FormValues.Add("FormEffectiveDate", effectiveDate);
            FormValues.Add("NewInsuredName", ReportData.Risk.NameAndAddress?.BusinessName);
            FormValues.Add("EndorsementEffectiveDate", effectiveDate);
        }
    }
}