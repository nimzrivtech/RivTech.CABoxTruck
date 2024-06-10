using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.Proration;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement
{
    public interface IEndorsementData
    {
        DateTime EffectiveDate { get; set; }
        IEndorsementChangeCollection EndorsementChanges { get; set; }
        Guid RiskDetailId { get; }
        int EndorsementNumber { get; set; }
        int VersionNumber { get;  }
        Guid? SourceRiskDetailId { get; set; }
        string SubmissionNumber { get; }

        Task RatePremiumAsync(IEndorsementDataPremiumRater _endorsementDataPremiumRater, int endorsementNumber, bool forIssuance);
        Task ApplyChangeAsync(IEndorsementChangeItem changeItem, IEndorsementChangeItemModifier modifier);
        void IncrementVersion();
        Task ProratePremiumsAsync(IEndorsementDataPremiumProrator endorsementDataPremiumProrator);
        Task<IEndorsementData> CloneAsync(IEndorsementDataCloner endorsementDataCloner);
    }
}
