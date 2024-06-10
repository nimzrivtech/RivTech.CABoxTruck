using System.Linq;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class PennsylvaniaAddCombFirstPartyBenefitsEndorsementPage : CommonValuesPage
    {
        public PennsylvaniaAddCombFirstPartyBenefitsEndorsementPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "PA/Pennsylvania Added and Combination First Party Benefits Endorsement.docx";
            PageOrder = 123;
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;

            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "PennsylvaniaAddCombFirstPartyBenefitsEndorsement")?.Other;

            if (formOtherContent != null)
            {
                var formInputs = JsonConvert.DeserializeObject<FormPAaddcombFPBDTO>(formOtherContent);
                effectiveDate = formInputs.UpdatedDate?.ToString(DateFormat);

                if (formInputs.AddedFirstPartyBenefits)
                    TemplateName = "PA/Pennsylvania Added and Combination First Party Benefits Endorsement_Added.docx";
                else
                    TemplateName = "PA/Pennsylvania Added and Combination First Party Benefits Endorsement_Comb.docx";

                FormValues.Add("MedicalExpense", $"{formInputs?.MedicalExpenseBenefits ?? 0:N0}");
                FormValues.Add("WorkLossUpTo", $"{formInputs?.WorkLossUpTo ?? 0:N0}");
                FormValues.Add("WorkLossPerMonth", $"{formInputs?.WorkLossPerMonth ?? 0:N0}");
                FormValues.Add("FuneralExpense", $"{formInputs?.FuneralExpenseBenefits ?? 0:N0}");
                FormValues.Add("AccidentalDeath", $"{formInputs?.AccidentalDeathBenefits ?? 0:N0}");

                FormValues.Add("MaximumTotalLimit", $"{formInputs?.MaximumTotalLimitAllBenefits ?? 0:N0}");
                FormValues.Add("FuneralExpense2", $"{formInputs?.CombFuneralExpenseBenefits ?? 0:N0}");
                FormValues.Add("AccidentalDeath2", $"{formInputs?.CombAccidentalDeathBenefits ?? 0:N0}");
            }

            base.ReplaceFormValues("EffectiveDate", effectiveDate);
            FormValues.Add("EndorsementEffectiveDate", effectiveDate);
        }
    }
}