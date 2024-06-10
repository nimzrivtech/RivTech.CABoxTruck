using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Policy;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.DTO.Billing.Fees;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.Billing.Application.DTO.Views;
using RivTech.CABoxTruck.WebService.DTO.Billing.Taxes;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Billing
{
    public interface IBillingService
    {
        Task UpdatePaymentOptions(Guid riskDetailId, int? optionId = null);
        List<FeeDetails> GetFeeDetailsByState(string stateCode);

        BillingBindResponseDTO ProcessBillingBind(
            string policyNumber, 
            DateTime effectiveDate, 
            RiskDetail riskDetail, 
            Binding bindingDetails, 
            RiskCoverage riskCoverage, 
            List<Vehicle> vehicles, 
            string authorizeId = null);
        Task<BillingBindResponseDTO> ProcessBillingBindEndorsement(
            Guid riskId,
            DateTime effectiveDate,
            List<EndorsementPendingChange> pendingChange,
            DateTime? policyEffectiveDate,
            string authorizeId = null);
        Task<string> AuthorizePayment(string policyNumber, DateTime effectiveDate, RiskDetail riskDetail, Binding bindingDetails, RiskCoverage riskCoverage, List<Vehicle> vehicles);
        Task<BillingComputeRiskTaxResponseDTO> ProcessComputeRiskTax(Guid riskId, DateTime effectiveDate, string stateCode, List<TransactionDetailDTO> transactionDetails, decimal? installmentCount);

        Task<List<InstallmentAndInvoiceDTO>> GetInstallmentSchedule(RiskDTO risk);
        Task<List<InstallmentAndInvoiceDTO>> GetEndorsementInstallmentSchedule(Guid riskId);
        Task<BillingCancellationDTO> ProcessBillingCancellation(Guid riskId, DateTime effectiveDate, IEnumerable<CancellationBreakdown> breakdowns, bool isFlatCancel);
        Task ProcessBillingReinstatement(Guid riskId, IEnumerable<ReinstatementBreakdown> resinstatementBreakdowns, DateTime? policyEffectiveDate, bool isFlatCancel);
        //void ProcessBillingCancellation();

        Task<List<PnocDTO>> GetRisksForNoticeOfCancellation(DateTime effectiveDate);
        Task<List<Guid>> GetRisksForReinstate(DateTime effectiveDate);
        Task<BillingSummaryDTO> GetBillingSummaryAsync(Guid riskId);
        Task<IEnumerable<BillingFeeDTO>> GetBillingFeesAsync(Guid riskId);
        Task<PaymentDetailsViewDto> GetPaymentDetails(Guid riskId, Guid paymentId);
        Task<IEnumerable<InvoiceDTO>> GetInvoicesAsync(Guid riskId);
        Task<IEnumerable<PaymentDTO>> GetPaymentsAsync(Guid id);
        Task<List<InvoiceDTO>> GetInvoicesByDateRange(DateTime fromDate, DateTime toDate);
        Task<StateTax> GetStateTaxAsync(DateTime effectiveDate, string stateCode);
        Task UpdateTransactionPolicyHistoryId(Guid riskId, long policyHistoryId);
    }
}
