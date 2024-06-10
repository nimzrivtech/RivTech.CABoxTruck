using System;
using System.Linq;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class MCS90Page : CommonValuesPage
    {
        public MCS90Page(ReportData reportData) : base(reportData)
        {
            TemplateName = "MCS90.docx";
            PageOrder = 11;
            FormId = "MCS90";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "MCS90")?.Other;
            var formInputs = formOtherContent != null
                ? JsonConvert.DeserializeObject<Mcs90DTO>(formOtherContent)
                : GetMcs90Defaults();

            base.ReplaceFormValues("EffectiveDate", formInputs?.EffectiveDate.ToString("MM/dd/yyyy"));

            FormValues.Add("USDOTNumber", formInputs?.UsDotNumber);
            FormValues.Add("DateReceived", formInputs?.DateReceived.ToString(DateFormat));

            FormValues.Add("IssuedTo", formInputs?.IssuedTo);
            FormValues.Add("IssuingState",
                ReportData.DataSets["States"].FirstOrDefault(s => s.StateCode == formInputs?.State)?.FullStateName
                    ?.Replace("[E]", ""));

            FormValues.Add("DatedAt", formInputs?.DatedAt);
            FormValues.Add("DatedThis", $"{formInputs?.PolicyDateDay:d}{GetDayOrdinal(formInputs?.PolicyDateDay ?? 0)}");
            FormValues.Add("DatedOf", formInputs?.PolicyDateMonth);
            FormValues.Add("DatedYear", formInputs?.PolicyDateYear);

            FormValues.Add("InsuranceCompany", formInputs?.InsuranceCompany);
            FormValues.Add("CountersignedBy", formInputs?.CounterSignedBy);
            FormValues.Add("AccidentLimit", $"{formInputs?.AccidentLimit ?? 0:N0}");
        }

        private static string GetDayOrdinal(int day)
        {
            return day % 10 == 1 && day % 100 != 11 ? "st"
                : day % 10 == 2 && day % 100 != 12 ? "nd"
                : day % 10 == 3 && day % 100 != 13 ? "rd"
                : "th";
        }

        private Mcs90DTO GetMcs90Defaults()
        {
            var effectiveDate = ReportData.Risk?.BrokerInfo?.EffectiveDate ?? new DateTime();
            return new Mcs90DTO
            {
                UsDotNumber = ReportData.Risk?.FilingsInformation?.USDOTNumber,
                DateReceived = effectiveDate,
                IssuedTo = ReportData.Risk?.NameAndAddress?.BusinessName,
                State = ReportData.Risk?.NameAndAddress?.State,
                DatedAt = "Omaha, NE",
                PolicyDateDay = effectiveDate.Day,
                PolicyDateMonth = effectiveDate.ToString("MMMM"),
                PolicyDateYear = effectiveDate.ToString("yyyy"),
                PolicyNumber = ReportData.Risk?.PolicyNumber,
                EffectiveDate = effectiveDate,
                InsuranceCompany = "Texas Insurance Company",
                CounterSignedBy = "Jeffrey A Silver",
                AccidentLimit = 750000
            };
        }
    }
}