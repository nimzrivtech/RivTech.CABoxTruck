using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater.Mapping
{
    public class RaterInputMapperResolver : IRaterInputMapperResolver
    {
        private readonly IEnumerable<IRaterInputMapper> _raterInputMappings;

        public RaterInputMapperResolver(IEnumerable<IRaterInputMapper> raterInputMappings)
        {
            _raterInputMappings = raterInputMappings;
        }

        public IRaterInputMapper Resolve(string name)
        {
            return name switch
            {
                PolicyInputMapper.MODULE_NAME => _raterInputMappings.FirstOrDefault(x => x is PolicyInputMapper),
                BrokerInputMapper.MODULE_NAME => _raterInputMappings.FirstOrDefault(x => x is BrokerInputMapper),
                HistoricalCoverageInputMapper.MODULE_NAME => _raterInputMappings.FirstOrDefault(x => x is HistoricalCoverageInputMapper),
                ClaimsHistoryInputMapper.MODULE_NAME => _raterInputMappings.FirstOrDefault(x => x is ClaimsHistoryInputMapper),
                RiskSpecificsInputMapper.MODULE_NAME => _raterInputMappings.FirstOrDefault(x => x is RiskSpecificsInputMapper),
                RequestedCoveragesInputMapper.MODULE_NAME => _raterInputMappings.FirstOrDefault(x => x is RequestedCoveragesInputMapper),
                _ => null,
            };
        }
    }
}
