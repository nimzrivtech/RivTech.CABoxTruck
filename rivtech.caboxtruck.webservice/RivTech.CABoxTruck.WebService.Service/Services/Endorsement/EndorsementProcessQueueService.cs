using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.ProcessQueuing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement
{
    public interface IEndorsementProcessQueueService
    {
        Task DequeueByRiskAsync(Guid riskId);
        Task DequeueAsync(EndorsementProcessQueue queue);
        Task FailAsync(EndorsementProcessQueue queue);
        Task<EndorsementProcessQueue> GetByRiskAsync(Guid riskId);
        Task<IEnumerable<EndorsementProcessQueue>> GetUnstartedQueuesAsync();
        Task<EndorsementProcessQueue> QueueAsync(EndorsementProcessQueue request);
        Task StartQueueAsync(EndorsementProcessQueue queue);
        Task<bool> IsQueueStartedAsync(Guid riskId);
    }

    public class EndorsementProcessQueueService : IEndorsementProcessQueueService
    {
        private readonly IEndorsementProcessQueueRepository _endorsementProcessQueueRepository;

        public EndorsementProcessQueueService(
            IEndorsementProcessQueueRepository endorsementProcessQueueRepository
        )
        {
            _endorsementProcessQueueRepository = endorsementProcessQueueRepository;
        }

        public async Task<IEnumerable<EndorsementProcessQueue>> GetUnstartedQueuesAsync()
        {
            return await _endorsementProcessQueueRepository.GetUnstartedQueuesAsync();
        }

        public async Task<EndorsementProcessQueue> QueueAsync(EndorsementProcessQueue request)
        {
            var existingQueue = await _endorsementProcessQueueRepository.GetActiveByRiskIdAsync(request.RiskId);
            if (existingQueue != null)
                return existingQueue;

            var queue = EndorsementProcessQueue.Create(request);
            return await _endorsementProcessQueueRepository.CreateAsync(queue);
        }

        public async Task StartQueueAsync(EndorsementProcessQueue queue)
        {
            queue.Start();
            await _endorsementProcessQueueRepository.UpdateAsync(queue);
        }

        public async Task DequeueAsync(EndorsementProcessQueue queue)
        {
            if (queue is null)
                return;
            await _endorsementProcessQueueRepository.DeleteAsync(queue);
        }

        public async Task FailAsync(EndorsementProcessQueue queue)
        {
            queue.Fail();
            await _endorsementProcessQueueRepository.UpdateAsync(queue);
        }

        public async Task<EndorsementProcessQueue> GetByRiskAsync(Guid riskId)
        {
            return await _endorsementProcessQueueRepository.GetActiveByRiskIdAsync(riskId);
        }

        public async Task DequeueByRiskAsync(Guid riskId)
        {
            var queue = await GetByRiskAsync(riskId);
            await DequeueAsync(queue);
        }

        public async Task<bool> IsQueueStartedAsync(Guid riskId)
        {
            var queue = await GetByRiskAsync(riskId);
            return queue.InProgress;
        }
    }
}
