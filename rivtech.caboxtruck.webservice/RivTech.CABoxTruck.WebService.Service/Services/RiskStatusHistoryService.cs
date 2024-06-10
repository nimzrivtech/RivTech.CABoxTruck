using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class RiskStatusHistoryService : IRiskStatusHistoryService
    {
        private readonly IRiskStatusHistoryRepository _riskStatusHistoryRepository;

        public RiskStatusHistoryService(IRiskStatusHistoryRepository riskStatusHistoryRepository)
        {
            _riskStatusHistoryRepository = riskStatusHistoryRepository;
        }

        public async Task<bool> InsertAsync(RiskStatusHistory model)
        {
            await _riskStatusHistoryRepository.AddAsync(model);
            return true;
        }
    }
}