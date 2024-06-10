using RivTech.CABoxTruck.WebService.Data.Entity.Endorsements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement
{
    public interface IEndorsementProcessStatusLogRepository
    {
        Task<EndorsementProcessStatusLog> CreateAsync(EndorsementProcessStatusLog statusLog);
        Task DeleteAsync(IEnumerable<EndorsementProcessStatusLog> logs);
        Task<bool> ExistsAsync(Guid riskId);
        Task<IEnumerable<EndorsementProcessStatusLog>> GetAsync(Guid riskId);
    }
}
