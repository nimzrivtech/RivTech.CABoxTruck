using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IInsuredRepository : IRepository<Insured, Guid>
    {
        Task<Insured> GetInsuredAsync(Guid id);
    }
}
