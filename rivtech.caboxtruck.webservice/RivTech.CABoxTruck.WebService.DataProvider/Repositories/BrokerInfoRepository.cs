using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class BrokerInfoRepository : BaseRepository<BrokerInfo, Guid>, IBrokerInfoRepository
    {
        private readonly IAgencyRepository _agencyRepository;
        private readonly IAgentRepository _agentRepository;
        private readonly IRetailerRepository _retailerRepository;
        private readonly IRetailerAgentRepository _retailerAgentRepository;
        public BrokerInfoRepository(AppDbContext context, 
            IAgencyRepository agencyRepository,
            IAgentRepository agentRepository,
            IRetailerRepository retailerRepository,
            IRetailerAgentRepository retailerAgentRepository
            )
           : base(context)
        {
            _agencyRepository = agencyRepository;
            _agentRepository = agentRepository;
            _retailerRepository = retailerRepository;
            _retailerAgentRepository = retailerAgentRepository;
        }

        public async Task<BrokerInfo> GetByRiskIdAllAsync(Guid id)
        {
            return await _context.BrokerInfo.FirstOrDefaultAsync(x => x.RiskId == id);
        }

        public async Task<BrokerInfo> GetByRiskIdAllIncludeAsync(Guid id)
        {
            //var result = _context.BrokerInfo.Include(broker => broker.Agent).ThenInclude(agent => agent.Entity)
            //    .Include(broker => broker.SubAgent).ThenInclude(subAgent => subAgent.Entity)
            //    .Include(broker => broker.Agency).ThenInclude(agency => agency.Entity)
            //    .Include(broker => broker.SubAgency).ThenInclude(subAgency => subAgency.Entity).FirstOrDefaultAsync(x => x.RiskId == id);
            var result = await _context.BrokerInfo.AsNoTracking().FirstOrDefaultAsync(x => x.RiskId == id);
            if (result != null)
            {
                // Get Agency Details
                if (result.AgencyId.HasValue)
                    result.Agency = await _agencyRepository.GetAgency(result.AgencyId.Value);

                // Get Agent Details
                if (result.AgentId.HasValue)
                    result.Agent = await _agentRepository.GetAgent(result.AgentId.Value);

                // Get Retailer Details
                if (result.SubAgencyId.HasValue)
                {
                    var retailer = await _retailerRepository.FindAsync(x => x.Id == result.SubAgencyId, i => i.Entity);
                    result.SubAgency = new SubAgency()
                    {
                        Id = retailer.Id,
                        EntityId = retailer.EntityId,
                        Entity = retailer.Entity
                    };
                }    

                // Get Retailer Agent Details
                if (result.SubAgentId.HasValue)
                {
                    var retailerAgent = await _retailerAgentRepository.FindAsync(x => x.Id == result.SubAgentId, i => i.Entity);
                    result.SubAgent = new SubAgent()
                    {
                        Id = retailerAgent.Id,
                        EntityId = retailerAgent.EntityId,
                        Entity = retailerAgent.Entity
                    };
                }
            }

            return result;
        }
    }
}
