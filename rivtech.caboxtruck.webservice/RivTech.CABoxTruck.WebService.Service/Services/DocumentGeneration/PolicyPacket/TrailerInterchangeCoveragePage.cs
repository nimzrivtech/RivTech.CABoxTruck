using System.Linq;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class TrailerInterchangeCoveragePage : CommonValuesPage
    {
        public TrailerInterchangeCoveragePage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Trailer Interchange Coverage.docx";
            PageOrder = 30;
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;
            var endorsementEffectiveDate = EndorsementEffectiveDate;
            var formOtherContent = ReportData.Risk.RiskForms
                .FirstOrDefault(f => f.FormId == "TrailerInterchangeCoverage")?.Other;

            if (formOtherContent == null) return;

            var formInputs = JsonConvert.DeserializeObject<TrailerInterchangeDTO>(formOtherContent);

            effectiveDate = formInputs.UpdatedDate?.ToString(DateFormat);

            FormValues.Add("FormEffectiveDate", effectiveDate);
            FormValues.Add("EndorsementEffectiveDate", effectiveDate);

            FormValues.Add("CompreLimitOfInsurance", $"{formInputs?.CompreLimitOfInsurance ?? 0:N0}");
            FormValues.Add("CompreDeductible", $"{formInputs?.CompreDeductible ?? 0:N0}");
            FormValues.Add("ComprePremium", $"{formInputs?.ComprePremium ?? 0:N0}");

            FormValues.Add("LossLimitOfInsurance", $"{formInputs?.LossLimitOfInsurance ?? 0:N0}");
            FormValues.Add("LossDeductible", $"{formInputs?.LossDeductible ?? 0:N0}");
            FormValues.Add("LossPremium", $"{formInputs?.LossPremium ?? 0:N0}");

            FormValues.Add("CollisionLimitOfInsurance", $"{formInputs?.CollisionLimitOfInsurance ?? 0:N0}");
            FormValues.Add("CollisionDeductible", $"{formInputs?.CollisionDeductible ?? 0:N0}");
            FormValues.Add("CollisionPremium", $"{formInputs?.CollisionPremium ?? 0:N0}");

            FormValues.Add("FireLimitOfInsurance", $"{formInputs?.FireLimitOfInsurance ?? 0:N0}");
            FormValues.Add("FireDeductible", $"{formInputs?.FireDeductible ?? 0:N0}");
            FormValues.Add("FirePremium", $"{formInputs?.FirePremium ?? 0:N0}");

            FormValues.Add("FireTheftLimitOfInsurance", $"{formInputs?.FireTheftLimitOfInsurance ?? 0:N0}");
            FormValues.Add("FireTheftDeductible", $"{formInputs?.FireTheftDeductible ?? 0:N0}");
            FormValues.Add("FireTheftPremium", $"{formInputs?.FireTheftPremium ?? 0:N0}");
        }
    }
}