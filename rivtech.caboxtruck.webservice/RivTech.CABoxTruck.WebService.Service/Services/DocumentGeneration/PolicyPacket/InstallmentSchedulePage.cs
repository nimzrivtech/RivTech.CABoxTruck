using System.Collections.Generic;
using System.Linq;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class InstallmentSchedulePage : CommonValuesPage
    {
        public InstallmentSchedulePage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Installment Schedule.docx";
            PageOrder = 4;
            FormId = "InstallmentSchedule";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var installments = ReportData.DataSets["InstallmentSchedule"].Cast<InstallmentAndInvoiceDTO>().ToList();

            var installmentSchedules = new List<Dictionary<string, string>>();
            foreach (var installment in installments)
            {
                decimal otherFees = installment.Fee - installment.FutureInstallmentFee;
                decimal installmentFee = installment.Fee;
                decimal installmentPremium = installment.Premium;

                if (installment.FutureInstallmentFee != installment.Fee)
                {
                    installmentPremium += otherFees;
                    installmentFee = installment.FutureInstallmentFee;
                }

                installmentSchedules.Add(new Dictionary<string, string>
                {
                    {"Table_Installments_DueDate", $"{installment.DueDate.ToString(DateFormat)}"},
                    {"Table_Installments_PremiumDue", installmentPremium.ToCurrency()},
                    {"Table_Installments_Fee", installmentFee.ToCurrency()},
                    {"Table_Installments_TaxDue", installment.Tax.ToCurrency()},
                    {"Table_Installments_TotalDue", installment.TotalBilled.ToCurrency()}
                });
            }

            TableValues = new Dictionary<string, List<Dictionary<string, string>>>
            {
                {"Installments_", installmentSchedules}
            };
        }

    }

}