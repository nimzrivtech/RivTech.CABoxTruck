using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class TrailerInterchangeCoverageALPage : CommonValuesPage
    {
        public TrailerInterchangeCoverageALPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Trailer Interchange Coverage AL.docx";
            PageOrder = 42;
            FormId = "TrailerInterchangeCoverageAL";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;
            var formOtherContent = ReportData.Risk.RiskForms
                .FirstOrDefault(f => f.FormId == "TrailerInterchangeCoverageAL")?.Other;

            var trailerInterchangeLimitValues = Tuple.Create("", "");

            if (formOtherContent == null)
            {
                var riskCoverage = ReportData.Risk.RiskCoverages[0];

                var trailerInterchangeLimitValue = ReportData.DataSets["TrailerInterchangeLimits"]?.Find(c => c.Id == riskCoverage?.TrailerInterchangeLimitId)?.LimitValue;
                trailerInterchangeLimitValues = ParseTrailerLimitValue(trailerInterchangeLimitValue);
            }
            else
            {
                var formInputs = JsonConvert.DeserializeObject<TrailerInterchangeALDTO>(formOtherContent);
                if (ReportData.Endorsement != null || formInputs?.UpdatedDate != null)
                    effectiveDate = formInputs.UpdatedDate?.ToString(DateFormat);

                var trailerInterchangeLimitValue = ReportData.DataSets["TrailerInterchangeLimits"]?.Find(c => c.Id == formInputs?.TrailerInterchangeLimitId)?.LimitValue;
                trailerInterchangeLimitValues = ParseTrailerLimitValue(trailerInterchangeLimitValue);
            }

            FormValues.Add("FormEffectiveDate", effectiveDate);
            FormValues.Add("EndorsementEffectiveDate", effectiveDate);
            FormValues.Add("CompreLimitOfInsurance", trailerInterchangeLimitValues.Item1);
            FormValues.Add("CompreDeductible", trailerInterchangeLimitValues.Item2);

            FormValues.Add("CollisionLimitOfInsurance", trailerInterchangeLimitValues.Item1);
            FormValues.Add("CollisionDeductible", trailerInterchangeLimitValues.Item2);
        }

        private Tuple<string,string> ParseTrailerLimitValue(string trailerLimitValue)
        {
            string pattern = @"(\d+) WITH (\d+)";

            MatchCollection matches = Regex.Matches(trailerLimitValue, pattern);
            var matched = matches.FirstOrDefault();

            if (matched != null)
            {
                var value1 = matched?.Groups[1]?.Value;
                var value2 = matched?.Groups[2]?.Value;

                if(value1 != null)
                    value1 = int.Parse(matched.Groups[1].Value).ToString("N0");

                if (value2 != null)
                    value2 = int.Parse(matched.Groups[2].Value).ToString("N0");

                return Tuple.Create(value1 ?? "", value2 ?? "");
            }

            return Tuple.Create("", "");
        }
    }
}