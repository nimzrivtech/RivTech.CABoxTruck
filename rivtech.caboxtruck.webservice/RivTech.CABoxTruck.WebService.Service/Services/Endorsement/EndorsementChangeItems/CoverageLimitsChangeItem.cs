using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class CoverageLimitsChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(CoverageLimitsChangeItem);

        public string DisplayText => "";

        public Int16? AutoBILimitId { get; set; }
        public Int16? AutoPDLimitId { get; set; }
        public Int16? LiabilityDeductibleId { get; set; }
        public Int16? MedPayLimitId { get; set; }
        public Int16? PIPLimitId { get; set; }
        public Int16? UMBILimitId { get; set; }
        public Int16? UMPDLimitId { get; set; }
        public Int16? UIMBILimitId { get; set; }
        public Int16? UIMPDLimitId { get; set; }
        public Int16? CoveredSymbolsId { get; set; }
        public Int16? SpecifiedPerils { get; set; }
        public Int16? CompDeductibleId { get; set; }
        public Int16? FireDeductibleId { get; set; }
        public Int16? CollDeductibleId { get; set; }
        public Int16? GLBILimitId { get; set; }
        public Int16? CargoLimitId { get; set; }
        public Int16? RefCargoLimitId { get; set; }
        public Int16? NumAddtlInsured { get; set; }
        public Int16? WaiverSubrogation { get; set; }
        public Int16? PrimeNonContribEndorsement { get; set; }
        public Int16? TrailerInterchangeLimitId { get; set; }
        public Int16? NonOwnedTrailerLimitId { get; set; }
        public Int16? TrailerTypeLimitId { get; set; }
        public Int16? PollutionCovLimitId { get; set; }

        public Int32? EndorsementNumber { get; set; }
        public Int16? OptionNumber { get; set; }
        public Int16? ALSymbolId { get; set; }
        public Int16? PDSymbolId { get; set; }
        public Int16? LiabilityDeductibleSymbolId { get; set; }
        public Int16? MedPayLimitSymbolId { get; set; }
        public Int16? PIPLimitSymbolId { get; set; }
        public Int16? UMBILimitSymbolId { get; set; }
        public Int16? UMPDLimitSymbolId { get; set; }
        public Int16? UIMBILimitSymbolId { get; set; }
        public Int16? UIMPDLimitSymbolId { get; set; }
        public Int16? CompDeductibleSymbolId { get; set; }
        public Int16? FireDeductibleSymbolId { get; set; }
        public Int16? CollDeductibleSymbolId { get; set; }
        public Int16? GLBILimitSymbolId { get; set; }
        public Int16? CargoLimitSymbolId { get; set; }
        public Int16? RefCargoLimitSymbolId { get; set; }
        public Int16? TrailerInterchangeLimitSymbolId { get; set; }
        public Int16? NonOwnedTrailerLimitSymbolId { get; set; }

        public string BrokerCommisionAL { get; set; }
        public string BrokerCommisionPD { get; set; }
        public decimal? DepositPct { get; set; }
        public decimal? DepositPremium { get; set; }
        public int? NumberOfInstallments { get; set; }
        public string SubjectivitiesId { get; set; }
        public RiskCoveragePremium RiskCoveragePremium { get; set; }
    }
}
