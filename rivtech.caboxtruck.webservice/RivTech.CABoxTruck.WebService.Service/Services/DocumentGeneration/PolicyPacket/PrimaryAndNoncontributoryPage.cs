using System;
using System.Collections.Generic;
using System.Linq;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class PrimaryAndNoncontributoryPage : CommonValuesPage
    {
        public PrimaryAndNoncontributoryPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Primary And Noncontributory.docx";
            PageOrder = 19;
            FormId = "PrimaryAndNoncontributory";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;

            var pncEntities = new List<Dictionary<string, string>>();
            if (!IsEndorsement && ReportData.Risk.BlanketCoverage.IsPrimaryNonContributory.GetValueOrDefault())
            {
                pncEntities.Add(new Dictionary<string, string> {{ "Table_Entities_Name", BlanketMessage}});
            }
            else
            {
                var pnc = ReportData.Risk.AdditionalInterest.FirstOrDefault(ai => ai.Id == new Guid(ReportData.Id));
                if (pnc != null)
                {
                    effectiveDate = pnc.EffectiveDate?.ToString(DateFormat);
                    if (AdditionalInterestChanged(pnc.Id)) effectiveDate = ReportData.Endorsement.EffectiveDate.ToString(DateFormat);
                    
                    pncEntities.Add(new Dictionary<string, string> 
                        {{"Table_Entities_Name", $"{pnc.Name}\n{pnc.Address}, {pnc.City}, {pnc.State} {pnc.ZipCode}"}}
                    );
                }
            }

            TableValues = new Dictionary<string, List<Dictionary<string, string>>> {{"Entities_", pncEntities}};

            FormValues.Add("FormEffectiveDate", effectiveDate);
            FormValues.Add("EndorsementEffectiveDate", effectiveDate);
        }
    }
}