using RivTech.CABoxTruck.WebService.DTO.Billing.Taxes;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Billing
{
    public interface IStateTaxRepository
    {
        StateTax Get(string stateCode, DateTime effectiveDate);
        Task<StateTax> GetAsync(string stateCode, DateTime effectiveDate);
    }
}
