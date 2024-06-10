using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Premiums
{
    public class VehicleCoveragePremium : CoveragePremiumBase
    {
        public static readonly VehicleCoveragePremiumBuilder Builder = new VehicleCoveragePremiumBuilder();

        private VehicleCoveragePremium() { }
        private VehicleCoveragePremium(DateRange coveragePeriod)
        {
            CoveragePeriod = coveragePeriod;
        }

        public VehicleCoveragePremium(int id, DateRange coveragePeriod, DateRange policyPeriod, Premium alPremium)
        {
            Id = id;
            CoveragePeriod = coveragePeriod;
            ALPremium = alPremium ?? throw new ArgumentNullException(nameof(alPremium));
            RecalculateProratedPremium(policyPeriod);
        }

        public Premium ALPremium { get; private set; }
        public Premium PDPremium { get; set; }
        public Premium CargoPremium { get; set; }
        public Premium CompFTPremium { get; set; }
        public Premium CollisionPremium { get; set; }
        public Premium RefrigerationPremium { get; set; }

        public void UpdatePremiums(VehicleCoveragePremium coveragePremium)
        {
            ALPremium = ALPremium.ChangeAnnual(coveragePremium.ALPremium.Annual);
            PDPremium = PDPremium.ChangeAnnual(coveragePremium.PDPremium.Annual);
            CargoPremium = CargoPremium.ChangeAnnual(coveragePremium.CargoPremium.Annual);
            CompFTPremium = CompFTPremium.ChangeAnnual(coveragePremium.CompFTPremium.Annual);
            CollisionPremium = CollisionPremium.ChangeAnnual(coveragePremium.CollisionPremium.Annual);
            Premium = Premium.ChangeAnnual(coveragePremium.Premium.Annual);
        }

        public IEnumerable<RiskVehiclePremium> RiskVehiclePremiums { get; set; }

        public override void RecalculateProratedPremium(DateRange policyPeriod)
        {
            ALPremium = ALPremium.Prorate(CoveragePeriod, policyPeriod);
            PDPremium = (PDPremium ?? Premium.Empty()).Prorate(CoveragePeriod, policyPeriod);
            CargoPremium = (CargoPremium ?? Premium.Empty()).Prorate(CoveragePeriod, policyPeriod);
            Premium = new Premium(
                ALPremium.Annual + PDPremium.Annual + CargoPremium.Annual,
                ALPremium.Prorated + PDPremium.Prorated + CargoPremium.Prorated
            );

            CompFTPremium = (CompFTPremium ?? Premium.Empty()).Prorate(CoveragePeriod, policyPeriod);
            CollisionPremium = (CollisionPremium ?? Premium.Empty()).Prorate(CoveragePeriod, policyPeriod);
            RefrigerationPremium = (RefrigerationPremium ?? Premium.Empty()).Prorate(CoveragePeriod, policyPeriod);
        }

        public override CoveragePremiumBase Clone()
        {
            var premiumCov = new VehicleCoveragePremium(CoveragePeriod.Clone())
            {
                CoveragePeriod = CoveragePeriod.Clone(),
                Premium = Premium.Clone(),
                ALPremium = ALPremium.Clone(),
                PDPremium = PDPremium?.Clone(),
                CargoPremium = CargoPremium?.Clone(),
                CompFTPremium = CompFTPremium?.Clone(),
                CollisionPremium = CollisionPremium?.Clone(),
                RefrigerationPremium = RefrigerationPremium?.Clone()
            };
            premiumCov.UpdateTotalAnnualPremium();

            return premiumCov;
        }

        public override bool Equals(object obj)
        {
            var other = obj as VehicleCoveragePremium;
            return other != null
                    && other.Premium.Equals(Premium)
                    && other.ALPremium.Equals(ALPremium)
                    && other.PDPremium.Equals(PDPremium)
                    && other.CargoPremium.Equals(CargoPremium);
        }



        public class VehicleCoveragePremiumBuilder
        {
            VehicleCoveragePremium _instance = null;
            bool _hasProrated = false;

            /// <summary>
            /// Creates new instance/resets instance to be built.
            /// </summary>
            public VehicleCoveragePremiumBuilder New(DateRange coveragePeriod)
            {
                _instance = new VehicleCoveragePremium(coveragePeriod.Clone());
                return this;
            }

            public VehicleCoveragePremiumBuilder SetAL(decimal al)
            {
                _instance.ALPremium = new Premium(al);
                return this;
            }

            public VehicleCoveragePremiumBuilder SetPD(decimal pd)
            {
                _instance.PDPremium = new Premium(pd);
                return this;
            }
            public VehicleCoveragePremiumBuilder SetCargo(decimal cargo)
            {
                _instance.CargoPremium = new Premium(cargo);
                return this;
            }

            public VehicleCoveragePremiumBuilder SetCompFT(decimal compFT)
            {
                _instance.CompFTPremium = new Premium(compFT);
                return this;
            }

            public VehicleCoveragePremiumBuilder SetColl(decimal coll)
            {
                _instance.CollisionPremium = new Premium(coll);
                return this;
            }

            public VehicleCoveragePremiumBuilder SetRefrigeration(decimal refrig)
            {
                _instance.RefrigerationPremium = new Premium(refrig);
                return this;
            }

            public VehicleCoveragePremiumBuilder UpdateTotalAnnualPremium()
            {
                _instance.UpdateTotalAnnualPremium();
                return this;
            }

            public VehicleCoveragePremiumBuilder Prorate(DateRange policyPeriod)
            {
                _instance.RecalculateProratedPremium(policyPeriod);
                _hasProrated = true;
                return this;
            }

            /// <summary>
            /// Returns instance created from builder.
            /// </summary>
            public VehicleCoveragePremium Build()
            {
                if (_instance.ALPremium is null)
                    throw new InvalidOperationException("Vehicle AL Premium must be set.");
                if (!_hasProrated)
                    throw new InvalidOperationException($"{nameof(VehicleCoveragePremium)} has not yet been prorated.");
                return _instance;
            }

        }

        private void UpdateTotalAnnualPremium()
        {
            var totalAnnual = ALPremium.Annual
                                + PDPremium.Annual
                                + CargoPremium.Annual;
            Premium = new Premium(totalAnnual);
        }
    }

}
