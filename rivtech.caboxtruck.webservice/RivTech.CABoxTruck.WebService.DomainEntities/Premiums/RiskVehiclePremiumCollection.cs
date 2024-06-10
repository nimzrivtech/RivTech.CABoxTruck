using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Premiums
{
    /// <summary>
    /// A collection of RiskVehiclePremium object. Must always be in descending order of effectivity. 
    /// </summary>
    public class RiskVehiclePremiumCollection : IEnumerable<RiskVehiclePremium>
    {
        public Stack<RiskVehiclePremium> RiskPremiums { get; private set; }

        public DateRange PolicyPeriod { get; private set; }

        public Guid VehicleMainId => RiskPremiums.FirstOrDefault()?.VehicleMainId ?? Guid.Empty;
        public Guid RiskDetailId => RiskPremiums.FirstOrDefault()?.RiskDetailId ?? Guid.Empty;

        public bool Cancelled { get; private set; } = false;
        public decimal TotalProratedPremium => RiskPremiums.Sum(x => x.CoveragePremium.Premium.Prorated);

        public decimal TotalProratedALPremium => RiskPremiums.Sum(x => x.CoveragePremium.ALPremium.Prorated);
        public decimal TotalProratedPDPremium => RiskPremiums.Sum(x => x.CoveragePremium.PDPremium.Prorated);
        public decimal TotalProratedCargoPremium => RiskPremiums.Sum(x => x.CoveragePremium.CargoPremium.Prorated);
        public decimal TotalProratedCompPremium => RiskPremiums.Sum(x => x.CoveragePremium.CompFTPremium.Prorated);
        public decimal TotalProratedCollPremium => RiskPremiums.Sum(x => x.CoveragePremium.CollisionPremium.Prorated);
        public decimal TotalProratedRefPremium => RiskPremiums.Sum(x => x.CoveragePremium.RefrigerationPremium.Prorated);

        private RiskVehiclePremiumCollection() { }
        public RiskVehiclePremiumCollection(DateRange policyPeriod)
        {
            RiskPremiums = new Stack<RiskVehiclePremium>();
            PolicyPeriod = policyPeriod;
        }
        public RiskVehiclePremiumCollection(RiskVehiclePremium newPremiumCoverage, DateRange policyPeriod) : this(policyPeriod)
        {
            RiskPremiums.Push(newPremiumCoverage);
        }

        public RiskVehiclePremiumCollection(IEnumerable<RiskVehiclePremium> vehiclePremiumList, DateRange policyPeriod)
        {
            if (vehiclePremiumList is null)
                throw new ArgumentNullException(nameof(vehiclePremiumList));
            vehiclePremiumList = vehiclePremiumList?.OrderBy(x => x.Order).ToList();
            RiskPremiums = new Stack<RiskVehiclePremium>();
            foreach (var riskPremium in vehiclePremiumList)
            {
                RiskPremiums.Push(riskPremium);
            }
            PolicyPeriod = policyPeriod;
            UpdateOrder();
        }

        public IEnumerable<RiskVehiclePremium> AsOrdered()
        {
            return this.OrderBy(x => x.Order);
        }

        public void Cancel(DateTime cancellationDate)
        {
            if (Cancelled)
                return;

            RiskVehiclePremium lastPoppedPremiumCov = null;
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

        public IEnumerator<RiskVehiclePremium> GetEnumerator()
        {
            return RiskPremiums.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return RiskPremiums.GetEnumerator();
        }

        public void AddOrUpdate(RiskVehiclePremium newRiskPremium)
        {
            if (!RiskPremiums.Any())
            {
                RiskPremiums.Push(newRiskPremium);
                return;
            }

            if (newRiskPremium.VehicleMainId != RiskPremiums.First().VehicleMainId)
                throw new ArgumentException($"{nameof(newRiskPremium)}'s {nameof(newRiskPremium.VehicleMainId)} does not match of collection items.");


            // Move each riskPremiumCoverage to skipped 
            // if newRiskPremium is out of sequence.
            var skippedPeriod = new Stack<RiskVehiclePremium>();
            RiskVehiclePremium currentRiskPremiumCoverage = RiskPremiums.Peek();
            while (currentRiskPremiumCoverage != null && newRiskPremium.IsEarlierThan(currentRiskPremiumCoverage))
            {
                skippedPeriod.Push(RiskPremiums.Pop());
                currentRiskPremiumCoverage = RiskPremiums.Any() ? RiskPremiums.Peek() : null;
            }

            RiskVehiclePremium precedingRiskPremium = RiskPremiums.Any() ? RiskPremiums.Peek() : null;
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
            var tempStack = new Stack<RiskVehiclePremium>();
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

        public RiskVehiclePremium Get(Guid riskDetailId, Guid vehicleMainId)
        {
            return RiskPremiums.FirstOrDefault(rvp => rvp.RiskDetailId == riskDetailId && rvp.VehicleMainId == vehicleMainId);
        }

        /// <summary>
        /// Creates new instances of each RiskVehiclePremium with new RiskDetailId.
        /// </summary>
        /// <param name="riskDetailId"></param>
        public void ChangeRiskDetail(Guid riskDetailId)
        {
            var tempStack = new Stack<RiskVehiclePremium>();
            while (RiskPremiums.Any() && RiskPremiums.Peek() != null)
            {
                tempStack.Push(RiskPremiums.Pop());
            }

            while (tempStack.Any() && tempStack.Peek() != null)
            {
                var newPremium = tempStack.Pop().Clone() as RiskVehiclePremium;
                newPremium.RiskDetailId = riskDetailId;
                RiskPremiums.Push(newPremium);
            }

            UpdateOrder();
        }

        public override bool Equals(object obj)
        {
            var other = obj as RiskVehiclePremiumCollection;
            return other != null
                && other.VehicleMainId == this.VehicleMainId;
        }

        public RiskVehiclePremiumCollection Clone()
        {
            var newCollection = new RiskVehiclePremiumCollection(PolicyPeriod);
            var tempStack = new Stack<RiskVehiclePremium>(RiskPremiums.ToList());

            while (tempStack.Any())
            {
                var rvp = (RiskVehiclePremium)tempStack.Pop().Clone();
                newCollection.AddOrUpdate(rvp);
            }

            foreach (var vehiclePremium in newCollection)
                vehiclePremium.CoveragePremium.RecalculateProratedPremium(PolicyPeriod);

            return newCollection;
        }

        public void TagAsFromReinstatement()
        {
            foreach(var riskPremium in this)
            {
                riskPremium.TagAsFromReinstatement();
            }
        }
    }
}
