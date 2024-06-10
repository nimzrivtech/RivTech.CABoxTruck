using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class EndorsementPendingChangeService : IEndorsementPendingChangeService
    {
        private readonly IEndorsementPendingChangeRepository _endorsementPendingChangeRepository;
        private readonly IMapper _mapper;
        private readonly ILog4NetService _errorLogservice;

        public EndorsementPendingChangeService(IEndorsementPendingChangeRepository endorsementPendingChangeRepository,
                                    IMapper mapper,
                                    ILog4NetService errorLogservice)
        {
            _mapper = mapper;
            _errorLogservice = errorLogservice;
            _endorsementPendingChangeRepository = endorsementPendingChangeRepository;
        }

        public async Task<List<EndorsementPendingChangeDTO>> GetAll(Guid riskDetailId)
        {
            var endorsementChanges = await _endorsementPendingChangeRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            return _mapper.Map<List<EndorsementPendingChangeDTO>>(endorsementChanges);
        }
    }
}
