using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class HiredPhysicalDamagePage : CommonValuesPage
    {
        public HiredPhysicalDamagePage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Hired Physical Damage Form.docx";
            PageOrder = 17;
            FormId = "HiredPhysicalDamage";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var agencyName = ReportData.Risk.BrokerInfo?.Agency?.Entity?.CompanyName;
            var brokerInfo = ReportData.Risk.BrokerInfo;
            var effectiveDate = brokerInfo?.EffectiveDate?.ToString(DateFormat);

            FormValues.Add("Agent", agencyName);
            //FormValues.Add("PolicyNumber2", ReportData.Risk?.PolicyNumber);
            FormValues.Add("PolicyPeriod2", $"{effectiveDate} - {brokerInfo?.ExpirationDate?.ToString(DateFormat)}");
            FormValues.Add("CompanyName", "Texas Insurance Company");

            var formOtherContent = ReportData.Risk.RiskForms
                .FirstOrDefault(f => f.FormId == "HiredPhysicalDamage")?.Other;

            if (formOtherContent == null) return;

            var formInputs = JsonConvert.DeserializeObject<HiredPhysdamDTO>(formOtherContent);
            FormValues.Add("AdditionalPremium", $"${formInputs?.EstimatedAdditionalPremium ?? 0:N0}");
            FormValues.Add("VehicleDays", formInputs?.VehicleDays.ToString());
            FormValues.Add("VehicleDays2", formInputs?.VehicleDays.ToString());
            FormValues.Add("RatePerDay", $"${formInputs?.RatePerDay ?? 0:N0}");
            FormValues.Add("GrossCombinedWeight", formInputs?.GrossCombinedWeight.ToString("N0"));
            FormValues.Add("AccidentLimitPerVehicle", $"${formInputs?.AccidentLimitPerVehicle ?? 0:N0}");
            FormValues.Add("Deductible", $"${formInputs?.Deductible ?? 0:N0}");
            FormValues.Add("PhysdamLimit", $"${formInputs?.PhysdamLimit ?? 0:N0}");

            if(ReportData.Endorsement != null || formInputs?.UpdatedDate != null)
                effectiveDate = formInputs?.UpdatedDate?.ToString(DateFormat);

            base.ReplaceFormValues("EffectiveDate", effectiveDate);
            FormValues.Add("EndorsementEffectiveDate", effectiveDate);
        }
    }
}