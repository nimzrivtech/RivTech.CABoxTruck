using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Premiums
{
    public interface IRiskVehiclePremiumService
    {
        Task UpdateVehiclePremiums(Guid riskDetailId, DateRange coveragePeriod, DateRange policyPeriod);
    }
}