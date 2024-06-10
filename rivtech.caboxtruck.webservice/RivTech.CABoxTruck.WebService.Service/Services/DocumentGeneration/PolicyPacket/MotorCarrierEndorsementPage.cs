using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO.Billing.Fees;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.Constants;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class MotorCarrierEndorsementPage : CommonValuesPage
    {
        public MotorCarrierEndorsementPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Motor Carrier Endorsement.docx";
            PageOrder = 48;
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var effectiveDate = PolicyEffectiveDate;

            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "MotorCarrierEndorsement")?.Other;
            
            if (formOtherContent != null)
            {
                var formValues = JsonConvert.DeserializeObject<FormBaseDTO>(formOtherContent);
                effectiveDate = formValues.UpdatedDate?.ToString(DateFormat);
            }

            if (ReportData.Risk?.DriverInfo?.IsHiringFromOthers == true)
            {
                var annualCostOfHire = ReportData.Risk?.DriverInfo?.AnnualCostOfHire ?? 0;
                if(annualCostOfHire > 0)
                {
                    FormValues.Add("PrimaryCov_Premium", "Included");
                    FormValues.Add("PrimaryCov_AnnualCostHire", $"{annualCostOfHire:N2}");
                }
            }

            if(Coverage.CompDeductibleId != null && Coverage.CompDeductibleId != LimitConstants.ComprehensiveNoCoverageId)
                FormValues.Add("IsComp", "X");

            if (Coverage.CollDeductibleId != null && Coverage.CollDeductibleId != LimitConstants.CollisionNoCoverageId)
                FormValues.Add("IsCol", "X");

            base.ReplaceFormValues("EffectiveDate2", effectiveDate);
            base.ReplaceFormValues("EffectiveDate", effectiveDate);
        }
    }
}