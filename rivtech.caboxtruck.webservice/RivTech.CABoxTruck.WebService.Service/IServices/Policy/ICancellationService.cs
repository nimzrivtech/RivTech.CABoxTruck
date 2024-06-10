using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Proration;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.Proration;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Policy
{
    public interface ICancellationService
    {
        Task<RiskAmounts> CancelPremiumFees(Guid riskDetailId, DateTime cancellationDate, bool? withShortRate = null, bool? isFlatCancel = null);
    }
}