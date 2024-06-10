using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement
{
    public class EndorsementPoint
    {
        public Guid RiskDetailId { get; set; }
        public DateTime EffectiveDate { get; } 
        public IEndorsementChangeCollection EndorsementChanges { get; }
        public int EndorsementNumber { get;  set; }

        public EndorsementPoint(Guid riskDetailId, DateTime effectiveDate, IEndorsementChangeCollection endorsementChanges, int endorsementNumber)
        {
            RiskDetailId = riskDetailId;
            EffectiveDate = effectiveDate;
            EndorsementChanges = endorsementChanges;
            EndorsementNumber = endorsementNumber;
        }

        public EndorsementPoint(DateTime effectiveDate, IEndorsementChangeCollection endorsementChanges, int endorsementNumber)
        {
            EffectiveDate = effectiveDate;
            EndorsementChanges = endorsementChanges;
            EndorsementNumber = endorsementNumber;
        }

        public override bool Equals(object obj)
        {
            var other = obj as EndorsementPoint;
            return other != null 
                    && RiskDetailId == other.RiskDetailId 
                    && EffectiveDate == other.EffectiveDate
                    && EndorsementChanges == other.EndorsementChanges
                    && EndorsementNumber == other.EndorsementNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RiskDetailId, EffectiveDate, EndorsementChanges, EndorsementNumber);
        }
    }
}
