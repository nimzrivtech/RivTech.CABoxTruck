using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.ProcessQueuing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement
{
    public interface IEndorsementProcessQueueRepository 
    {
        Task<EndorsementProcessQueue> CreateAsync(EndorsementProcessQueue queue);
        Task DeleteAsync(EndorsementProcessQueue queue);

        /// <summary>
        /// Must return the EndorsementProcessQueue with given RiskId and IsArchived is false.
        /// </summary>
        Task<EndorsementProcessQueue> GetActiveByRiskIdAsync(Guid riskId);

        /// <summary>
        /// Returns the list of EndorsementProcessQueue where IsArchived is false and no StartTime yet.
        /// </summary>
        Task<IEnumerable<EndorsementProcessQueue>> GetUnstartedQueuesAsync();
        Task UpdateAsync(EndorsementProcessQueue queue);
    }
}