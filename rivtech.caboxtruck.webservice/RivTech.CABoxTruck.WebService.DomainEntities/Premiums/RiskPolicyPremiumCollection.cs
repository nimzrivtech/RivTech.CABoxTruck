using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Premiums
{
    /// <summary>
    /// A collection of RiskPolicyPremium object. Must always be in descending order of effectivity.
    /// </summary>
    public class RiskPolicyPremiumCollection : IEnumerable<RiskPolicyPremium>
    {
        public Stack<RiskPolicyPremium> RiskPremiums { get; }

        public DateRange PolicyPeriod { get; }
        public bool Cancelled { get; protected set; }
        public Guid CurrentRiskDetailId => RiskPremiums.Peek().RiskDetailId;
        public string PremiumType => RiskPremiums.Peek().PremiumType;
        public decimal TotalProratedPremium => RiskPremiums.Sum(x => x.CoveragePremium.Premium.Prorated);


        private RiskPolicyPremiumCollection() { }

        public RiskPolicyPremiumCollection(IEnumerable<RiskPolicyPremium> riskPremiums, DateRange policyPeriod)
        {
            if (riskPremiums is null)
                throw new ArgumentNullException(nameof(riskPremiums));
            riskPremiums = riskPremiums.OrderBy(x => x.Order);
            RiskPremiums = new Stack<RiskPolicyPremium>();
            foreach (var riskremium in riskPremiums)
            {
                RiskPremiums.Push(riskremium);
            }
            PolicyPeriod = policyPeriod;

            UpdateOrder();
        }

        public RiskPolicyPremiumCollection(DateRange policyPeriod) : this(new List<RiskPolicyPremium>(), policyPeriod) { }

        public IEnumerator<RiskPolicyPremium> GetEnumerator()
        {
            return RiskPremiums.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return RiskPremiums.GetEnumerator();
        }

        public void Cancel(DateTime cancellationDate)
        {
            if (Cancelled)
                return;

            RiskPolicyPremium lastPoppedPremiumCov = null;
            while (RiskPremiums.Any() && cancellationDate.Date < RiskPremiums.Peek().EffectiveDate)
            {
                lastPoppedPremiumCov = RiskPremiums.Pop();
            }

            if (!RiskPremiums.Any())
            {
                // If cancellation date is earlier than effective date of initial coverage period,
                // re-insert the initial coverage with expired dates.
                lastPoppedPremiumCov.Expire(cancellationDate, PolicyPeriod);
                RiskPremiums.Push(lastPoppedPremiumCov);
            }
            else
            {
                var lastCoverage = RiskPremiums.Peek();
                if (lastCoverage.ExpirationDate < cancellationDate)
                {
                    // Do nothing when the last coverage's expiration date is already earlier
                    // than the cancellation date.
                    return;
                }

                // After popping the coverages later than the cancellation date,
                // expire the last coverage.
                lastCoverage = RiskPremiums.Pop();
                lastCoverage.Expire(cancellationDate, PolicyPeriod);
                RiskPremiums.Push(lastCoverage);
            }

            Cancelled = true;
        }

        public void AddOrUpdate(RiskPolicyPremium newRiskPremium)
        {
            if (!RiskPremiums.Any())
            {
                RiskPremiums.Push(newRiskPremium);
                return;
            }

            if (newRiskPremium.PremiumType != RiskPremiums.First().PremiumType)
                throw new ArgumentException($"{nameof(newRiskPremium)}'s {nameof(newRiskPremium.PremiumType)} does not match of collection items.");

            // Move each riskVehiclePremiumCoverage to skipped 
            // if newRiskVehiclePremiumCoverage is out of sequence.
            var skippedPeriod = new Stack<RiskPolicyPremium>();
            RiskPolicyPremium currentRiskPremiumCoverage = RiskPremiums.Peek();
            while (currentRiskPremiumCoverage != null && newRiskPremium.IsEarlierThan(currentRiskPremiumCoverage))
            {
                skippedPeriod.Push(RiskPremiums.Pop());
                currentRiskPremiumCoverage = RiskPremiums.Any()
                    ? RiskPremiums.Peek() : null;
            }

            RiskPolicyPremium precedingRiskPremium = RiskPremiums.Any() ? RiskPremiums.Peek() : null;
            bool hasPremiumsChanged = precedingRiskPremium?.IsDifferentAnnualPremium(newRiskPremium) ?? false;
            bool sameEffectiveDate = precedingRiskPremium?.EffectiveDate.Date.Equals(newRiskPremium.EffectiveDate.Date) ?? false;
            if (precedingRiskPremium is null || (hasPremiumsChanged || !sameEffectiveDate))
            {
                bool isOutOfSequence = skippedPeriod.Any();
                if (isOutOfSequence)
                {
                    // Create new coverage premium with annual premium, effective date, & expiration date.
                    newRiskPremium.Expire(skippedPeriod.Peek().EffectiveDate, PolicyPeriod);
                }
                else if (precedingRiskPremium != null)
                {
                    precedingRiskPremium.Expire(newRiskPremium.EffectiveDate, PolicyPeriod);
                }

                RiskPremiums.Push(newRiskPremium);
            }

            // Reapply coverage periods.
            while (skippedPeriod.Any())
            {
                RiskPremiums.Push(skippedPeriod.Pop());
            }

            UpdateOrder();
        }

        private void UpdateOrder()
        {
            var totalCount = RiskPremiums.Count;
            var tempStack = new Stack<RiskPolicyPremium>();
            while (RiskPremiums.Any())
            {
                var poppedRiskPremium = RiskPremiums.Pop();
                poppedRiskPremium.Order = totalCount--;
                tempStack.Push(poppedRiskPremium);
            }

            while (tempStack.Any())
            {
                RiskPremiums.Push(tempStack.Pop());
            }
        }

        public void ChangeRiskDetail(Guid riskDetailId)
        {
            var tempStack = new Stack<RiskPolicyPremium>();
            while (RiskPremiums.Any() && RiskPremiums.Peek() != null)
            {
                tempStack.Push(RiskPremiums.Pop());
            }

            while (tempStack.Any() && tempStack.Peek() != null)
            {
                var newPremium = tempStack.Pop().Clone() as RiskPolicyPremium;
                newPremium.RiskDetailId = riskDetailId;
                RiskPremiums.Push(newPremium);
            };

            UpdateOrder();
        }

        public RiskPolicyPremiumCollection Clone()
        {
            var newCollection = new RiskPolicyPremiumCollection(PolicyPeriod);
            var tempStack = new Stack<RiskPolicyPremium>(RiskPremiums.ToList());

            while (tempStack.Any())
            {
                var rvp = (RiskPolicyPremium)tempStack.Pop().Clone();
                newCollection.AddOrUpdate(rvp);
            }

            foreach (var vehiclePremium in newCollection)
                vehiclePremium.CoveragePremium.RecalculateProratedPremium(PolicyPeriod);

            return newCollection;

        }

        public void TagAsFromReinstatement()
        {
            foreach(var premium in this)
            {
                premium.TagAsFromReinstatement();
            }
        }
    }
}
