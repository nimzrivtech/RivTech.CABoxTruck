using log4net.Core;
using Microsoft.Extensions.Logging;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Endorsements;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.Service.Services.Rater.Mapping;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement
{
    public interface IEndorsementProcessStatusLogService
    {
        Task DeleteAsync(Guid riskId);
        Task<IEnumerable<EndorsementProcessStatusLog>> GetByRiskAsync(Guid riskId);
        Task LogAsync(Guid riskId, string status);
    }

    public class EndorsementProcessStatusLogService : IEndorsementProcessStatusLogService
    {
        private readonly ILogger<EndorsementProcessStatusLogService> _logger;
        private readonly IEndorsementProcessStatusLogRepository _statusLogRepository;
        private readonly IRiskDetailRepository _riskDetail;
        private readonly IRiskService _riskService;

        public EndorsementProcessStatusLogService(
            ILogger<EndorsementProcessStatusLogService> logger,
            IRiskService riskService,
            IRiskDetailRepository riskDetail,
            IEndorsementProcessStatusLogRepository endorsementProcessStatusLogRepository
        )
        {
            _logger = logger;
            _riskService = riskService;
            _riskDetail = riskDetail;
            _statusLogRepository = endorsementProcessStatusLogRepository;
        }

        public async Task<IEnumerable<EndorsementProcessStatusLog>> GetByRiskAsync(Guid riskId)
        {
            return await _statusLogRepository.GetAsync(riskId);
        }

        public async Task LogAsync(Guid riskId, string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return;

            status = status.Replace("Endorsement 0", "Bind");

            var log = EndorsementProcessStatusLog.New(riskId, status);
            await _statusLogRepository.CreateAsync(log);
        }

        public async Task DeleteAsync(Guid riskId)
        {
            try
            {
                var logs = await GetByRiskAsync(riskId);

                if (logs.Count() == 0)
                {
                    throw new Exception("Resetting endorsement error. Please try again");
                }

                await _statusLogRepository.DeleteAsync(logs);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error occured while clearing status logs.");
            }
        }

    }
}
