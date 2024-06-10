using RivTech.CABoxTruck.WebService.Data.Entity.Billing;
using RivTech.CABoxTruck.WebService.Data.Entity.Policy;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Billing.Taxes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Billing
{
    public interface ITaxService
    {
        Task CalculateRiskTaxAsync(Guid guid, int? optionId = null, bool isEndorsement = false);
        Task<(TaxDetailsDTO, TaxDetailsDTO)> CalculateTaxPremiumChangesAsync(Guid riskDetailId, DateTime effectiveDate, decimal installment, List<EndorsementPendingChange> pendingChanges);

        Task<IEnumerable<TaxDetailsData>> GetTaxDetails(List<Guid> ids);
        Task<TaxDetailsData> GetTaxDetail(Guid id);
        Task ResetTaxDetailsAsync(Guid forEndoRiskDetailId);
    }
}
