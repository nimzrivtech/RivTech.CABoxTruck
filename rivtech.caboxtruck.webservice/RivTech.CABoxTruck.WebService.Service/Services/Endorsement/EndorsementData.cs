using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.Proration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement
{
    public class EndorsementData : IEndorsementData
    {
        public IEndorsementChangeCollection EndorsementChanges { get; set; }
        public RiskDetail RiskDetail { get; set; }
        public RiskDTO RiskDTO { get; set; }

        public Guid RiskDetailId => RiskDetail.Id;

        public string SubmissionNumber => RiskDetail.SubmissionNumber;
        public int RaterVersionId => RiskDTO.RaterVersionId;
        public DateTime EffectiveDate
        {
            get => RiskDetail.EndorsementEffectiveDate ?? DateTime.MinValue;
            set { RiskDetail.EndorsementEffectiveDate = value; }
        }
        public int EndorsementNumber
        {
            get { return RiskDetail.EndorsementNumber; }
            set { RiskDetail.EndorsementNumber = value; }
        }
        public int VersionNumber
        {
            get { return RiskDetail.VersionNumber; }
            set { RiskDetail.VersionNumber = value; }
        }
        public Guid? SourceRiskDetailId
        {
            get { return RiskDetail.SourceRiskDetailId; }
            set { RiskDetail.SourceRiskDetailId = value; }
        }
        public DateTime PolicyExpirationDate => RiskDTO.BrokerInfo.ExpirationDate.Value;
        public DateTime PolicyEffectiveDate => RiskDTO.BrokerInfo.EffectiveDate.Value;
        public decimal? GLAnnualPremium => RiskDTO.BoundRiskCoverage.RiskCoveragePremium.GeneralLiabilityPremium;

        public Guid RiskId => RiskDetail.RiskId;

        public EndorsementData(RiskDetail riskDetail, IEndorsementChangeCollection endorsementChanges)
        {
            RiskDetail = riskDetail ?? throw new ArgumentNullException(nameof(riskDetail));
            EndorsementChanges = endorsementChanges;
        }

        public void IncrementVersion()
        {
            RiskDetail.VersionNumber += 1;
        }

        public async Task ProratePremiumsAsync(IEndorsementDataPremiumProrator endorsementDataPremiumProrator)
        {
            await endorsementDataPremiumProrator.ProratePremiumsAsync(this);
        }

        public async Task RatePremiumAsync(IEndorsementDataPremiumRater endorsementDataPremiumRater, int endorsementNumber, bool forIssuance)
        {
            await endorsementDataPremiumRater.RateAsync(this, endorsementNumber, forIssuance);
        }

        public Task<IEndorsementData> CloneAsync(IEndorsementDataCloner endorsementDataCloner)
        {
            return endorsementDataCloner.CloneAsync(this);
        }

        public Task ApplyChangeAsync(IEndorsementChangeItem changeItem, IEndorsementChangeItemModifier modifier)
        {
            return modifier?.ModifyAsync(this, changeItem);
        }

    }
}
