using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Proration;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.Proration
{
    public interface IAmountFactory
    {
        Task<IAmount> CreateVehicleAL(RiskDetail riskDetail, Vehicle vehicle, DateRange policyPeriod);
        Task<IAmount> CreateVehiclePD(RiskDetail riskDetail, Vehicle vehicle, DateRange policyPeriod);
        Task<IAmount> CreateVehiclePDCollision(RiskDetail riskDetail, Vehicle vehicle, DateRange policyPeriod);
        Task<IAmount> CreateVehiclePDComprehensive(RiskDetail riskDetail, Vehicle vehicle, DateRange policyPeriod);
        Task<IAmount> CreateVehicleCargo(RiskDetail riskDetail, Vehicle vehicle, DateRange policyPeriod);
        Task<IAmount> CreateGeneralLiabilityAsync(RiskDetail riskDetail, RiskPolicyPremiumCollection policyPremiumsGL, DateRange policyPeriod);
        Task<IAmount> CreatePremiumAmountAsync(RiskDetail riskDetail, RiskPolicyPremiumCollection policyPremiumsGL, DateRange policyPeriod);
        Task<IAmount> CreateManuscriptAsync(RiskManuscripts manuscript, DateRange policyPeriod);
        IAmount CreateAdditionalPremium(RiskAdditionalInterest riskAdditionalInterest, AdditionalPremiumType type, bool isBlanket, bool? fromSubmission = null);

        Task<IEnumerable<IAmount>> CreatePDPremiumsAsync(RiskDetail riskDetail);
        Task<IEnumerable<IAmount>> CreateVehicleFees(Vehicle vehicle, RiskDetail riskDetail);
        Task<IEnumerable<IAmount>> CreateVehiclePremiums(RiskDetail riskDetail, Vehicle vehicle, DateRange policyPeriod);
    }
}
