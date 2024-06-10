using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater.ListParsers
{
    public class LimitsParser : IListParser
    {
        private readonly ILimitsService _limitsService;

        public LimitsParser(ILimitsService limitsService)
        {
            _limitsService = limitsService;
        }

        public string Type { get; internal set; }
        public string State { get; internal set; }

        public async Task<string> ParseAsync(object id, int? raterVersion = null)
        {
            if (id is null)
                return null;

            var limits = await _limitsService.GetPerStateAsync(State, raterVersion);
            var limitList = new List<LimitsDTO>();

            if (Type == LimitType.AutoBI)
                limitList = limits.AutoBILimits;
            else if (Type == LimitType.AutoPD)
                limitList = limits.AutoPDLimits;
            else if (Type == LimitType.LiabilityDeductible)
                limitList = limits.LiabilityDeductibleLimits;
            else if (Type == LimitType.PIP)
                limitList = limits.PipLimits;
            else if (Type == LimitType.MedPay)
                limitList = limits.MedPayLimits;
            else if (Type == LimitType.UMBI)
                limitList = limits.UMBILimits;
            else if (Type == LimitType.UMPD)
                limitList = limits.UMPDLimits;
            else if (Type == LimitType.UIMBI)
                limitList = limits.UIMBILimits;
            else if (Type == LimitType.UIMPD)
                limitList = limits.UIMPDLimits;
            else if (Type == LimitType.Comprehensive)
                limitList = limits.ComprehensiveLimits;
            else if (Type == LimitType.Collision)
                limitList = limits.CollisionLimits;
            else if (Type == LimitType.GL)
                limitList = limits.GlLimits;
            else if (Type == LimitType.Cargo)
                limitList = limits.CargoLimits;
            else if (Type == LimitType.RefBreakdown)
                limitList = limits.RefLimits;
            else if (Type == LimitType.TrailerInterchange)
                limitList = limits.TrailerInterchangeLimits;
            else if (Type == LimitType.NonOwnedTrailer)
                limitList = limits.NonOwnedTrailerLimits;
            else if (Type == LimitType.PollutionCov)
                limitList = limits.PollutionCovLimits;
            else if (Type == LimitType.TrailerType)
                limitList = limits.TrailerTypeLimits;

            return limitList?.Where(x => x.Id == (short)id)
                ?.Select(x => x.LimitValue)?.FirstOrDefault();
        }
    }
}
