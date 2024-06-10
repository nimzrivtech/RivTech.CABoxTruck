using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IBrokerInfoRepository : IRepository<BrokerInfo, Guid>
    {
        Task<BrokerInfo> GetByRiskIdAllAsync(Guid id);

        /// <summary>
        /// Returns BrokerInfo including the Agency/ Calls the Agency api.  
        /// </summary>
        Task<BrokerInfo> GetByRiskIdAllIncludeAsync(Guid id);
    }
}
