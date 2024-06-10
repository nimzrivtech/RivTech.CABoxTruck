using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class CoverageLimitsModifier : ChangeModifierBase<CoverageLimitsChangeItem>
    {
        private readonly IRiskCoverageRepository _riskCoverageRepository;
        private readonly IMapper _mapper;

        public CoverageLimitsModifier(
            IRiskCoverageRepository riskCoverageRepository,
            IMapper mapper
        ) {
            _riskCoverageRepository = riskCoverageRepository;
            _mapper = mapper;
        }

        public override async Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            var change = CastOriginalType(changeItem);

            var boundRiskCov = await _riskCoverageRepository.FindAsync(x => x.RiskDetailId == data.RiskDTO.Id && x.OptionNumber == data.RiskDTO.Binding.BindOptionId);

            if (boundRiskCov != null)
            {
                boundRiskCov.AutoBILimitId = change.AutoBILimitId ?? boundRiskCov.AutoBILimitId;
                boundRiskCov.AutoPDLimitId = change.AutoPDLimitId ?? boundRiskCov.AutoPDLimitId;
                boundRiskCov.LiabilityDeductibleId = change.LiabilityDeductibleId ?? boundRiskCov.LiabilityDeductibleId;
                boundRiskCov.MedPayLimitId = change.MedPayLimitId ?? boundRiskCov.MedPayLimitId;
                boundRiskCov.PIPLimitId = change.PIPLimitId ?? boundRiskCov.PIPLimitId;
                boundRiskCov.UMBILimitId = change.UMBILimitId ?? boundRiskCov.UMBILimitId;
                boundRiskCov.UMPDLimitId = change.UMPDLimitId ?? boundRiskCov.UMPDLimitId;
                boundRiskCov.UIMBILimitId = change.UIMBILimitId ?? boundRiskCov.UIMBILimitId;
                boundRiskCov.UIMPDLimitId = change.UIMPDLimitId ?? boundRiskCov.UIMPDLimitId;
                boundRiskCov.CoveredSymbolsId = change.CoveredSymbolsId ?? boundRiskCov.CoveredSymbolsId;
                boundRiskCov.CompDeductibleId = change.CompDeductibleId ?? boundRiskCov.CompDeductibleId;
                boundRiskCov.FireDeductibleId = change.FireDeductibleId ?? boundRiskCov.FireDeductibleId;
                boundRiskCov.CollDeductibleId = change.CollDeductibleId ?? boundRiskCov.CollDeductibleId;
                boundRiskCov.GLBILimitId = change.GLBILimitId ?? boundRiskCov.GLBILimitId;
                boundRiskCov.CargoLimitId = change.CargoLimitId ?? boundRiskCov.CargoLimitId;
                boundRiskCov.RefCargoLimitId = change.RefCargoLimitId ?? boundRiskCov.RefCargoLimitId;
                boundRiskCov.NumAddtlInsured = change.NumAddtlInsured ?? boundRiskCov.NumAddtlInsured;
                boundRiskCov.WaiverSubrogation = change.WaiverSubrogation ?? boundRiskCov.WaiverSubrogation;
                boundRiskCov.PrimeNonContribEndorsement = change.PrimeNonContribEndorsement ?? boundRiskCov.PrimeNonContribEndorsement;
                boundRiskCov.EndorsementNumber = change.EndorsementNumber ?? boundRiskCov.EndorsementNumber;
                boundRiskCov.ALSymbolId = change.ALSymbolId ?? boundRiskCov.ALSymbolId;
                boundRiskCov.PDSymbolId = change.PDSymbolId ?? boundRiskCov.PDSymbolId;
                boundRiskCov.LiabilityDeductibleSymbolId = change.LiabilityDeductibleSymbolId ?? boundRiskCov.LiabilityDeductibleSymbolId;
                boundRiskCov.MedPayLimitSymbolId = change.MedPayLimitSymbolId ?? boundRiskCov.MedPayLimitSymbolId;
                boundRiskCov.PIPLimitSymbolId = change.PIPLimitSymbolId ?? boundRiskCov.PIPLimitSymbolId;
                boundRiskCov.UMBILimitSymbolId = change.UMBILimitSymbolId ?? boundRiskCov.UMBILimitSymbolId;
                boundRiskCov.UMPDLimitSymbolId = change.UMPDLimitSymbolId ?? boundRiskCov.UMPDLimitSymbolId;
                boundRiskCov.UIMBILimitSymbolId = change.UIMBILimitSymbolId ?? boundRiskCov.UIMBILimitSymbolId;
                boundRiskCov.UIMPDLimitSymbolId = change.UIMPDLimitSymbolId ?? boundRiskCov.UIMPDLimitSymbolId;
                boundRiskCov.CompDeductibleSymbolId = change.CompDeductibleSymbolId ?? boundRiskCov.CompDeductibleSymbolId;
                boundRiskCov.FireDeductibleSymbolId = change.FireDeductibleSymbolId ?? boundRiskCov.FireDeductibleSymbolId;
                boundRiskCov.CollDeductibleSymbolId = change.CollDeductibleSymbolId ?? boundRiskCov.CollDeductibleSymbolId;
                boundRiskCov.GLBILimitSymbolId = change.GLBILimitSymbolId ?? boundRiskCov.GLBILimitSymbolId;
                boundRiskCov.CargoLimitSymbolId = change.CargoLimitSymbolId ?? boundRiskCov.CargoLimitSymbolId;
                boundRiskCov.RefCargoLimitSymbolId = change.RefCargoLimitSymbolId ?? boundRiskCov.RefCargoLimitSymbolId;
                boundRiskCov.BrokerCommisionAL = change.BrokerCommisionAL ?? boundRiskCov.BrokerCommisionAL;
                boundRiskCov.BrokerCommisionPD = change.BrokerCommisionPD ?? boundRiskCov.BrokerCommisionPD;
                boundRiskCov.DepositPct = change.DepositPct ?? boundRiskCov.DepositPct;
                boundRiskCov.DepositPremium = change.DepositPremium ?? boundRiskCov.DepositPremium;
                boundRiskCov.NumberOfInstallments = change.NumberOfInstallments ?? boundRiskCov.NumberOfInstallments;
                boundRiskCov.SubjectivitiesId = change.SubjectivitiesId ?? boundRiskCov.SubjectivitiesId;
                boundRiskCov.TrailerInterchangeLimitId = change.TrailerInterchangeLimitId ?? boundRiskCov.TrailerInterchangeLimitId;
                boundRiskCov.NonOwnedTrailerLimitId = change.NonOwnedTrailerLimitId ?? boundRiskCov.NonOwnedTrailerLimitId;
                boundRiskCov.TrailerTypeLimitId = change.TrailerTypeLimitId ?? boundRiskCov.TrailerTypeLimitId;
                boundRiskCov.PollutionCovLimitId = change.PollutionCovLimitId ?? boundRiskCov.PollutionCovLimitId;
                boundRiskCov.TrailerInterchangeLimitSymbolId = change.TrailerInterchangeLimitSymbolId ?? boundRiskCov.TrailerInterchangeLimitSymbolId;
                boundRiskCov.NonOwnedTrailerLimitSymbolId = change.NonOwnedTrailerLimitSymbolId ?? boundRiskCov.NonOwnedTrailerLimitSymbolId;

                _riskCoverageRepository.Update(boundRiskCov);
                await _riskCoverageRepository.SaveChangesAsync();

            }
        }
    }
}
