using System;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Premiums
{
    public abstract class RiskPremiumTransactionBase
    {
        protected RiskPremiumTransactionBase() { }

        protected RiskPremiumTransactionBase(Guid riskDetailId)
        {
            RiskDetailId = riskDetailId;
        }

        public Guid Id { get; set; }
        public Guid RiskDetailId { get; protected set; }
        public decimal PremiumChange { get; protected set; }
        public decimal CurrentPremium { get; protected set; }
        public decimal PreviousPremium { get; protected set; }
        public bool FromReinstatement { get; protected set; } = false;

        public virtual void CalculatePremiumChange()
        {
            PremiumChange =  CurrentPremium - PreviousPremium;
        }

        public void TagAsFromReinstatement()
        {
            FromReinstatement = true;
        }
    }
}