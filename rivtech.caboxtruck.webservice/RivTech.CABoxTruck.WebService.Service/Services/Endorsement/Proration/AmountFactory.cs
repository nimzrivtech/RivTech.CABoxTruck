using Azure.Storage.Blobs.Models;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Vehicles;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Proration;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using RivTech.CABoxTruck.WebService.DTO.Billing.Fees;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RiskData = RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.Proration
{
    public class AmountFactory : IAmountFactory
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IVehiclePremiumRepository _vehiclePremiumRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IRiskManuscriptsRepository _riskManuscriptsRepository;
        private readonly IPolicyHistoryRepository _policyHistoryRepository;
        private readonly IBindingRepository _bindingRepository;
        private readonly IRiskFormRepository _riskFormRepository;
        private readonly IBillingService _billingService;
        private readonly IRiskVehiclePremiumRepository _riskVehiclePremiumRepository;
        private readonly IRiskPolicyPremiumRepository _riskPolicyPremiumRepository;

        public AmountFactory(
            IVehicleRepository vehicleRepository,
            IVehiclePremiumRepository vehiclePremiumRepository,
            IRiskDetailRepository riskDetailRepository,
            IRiskManuscriptsRepository riskManuscriptsRepository,
            IPolicyHistoryRepository policyHistoryRepository,
            IBindingRepository bindingRepository,
            IRiskFormRepository riskFormRepository,
            IBillingService billingService,
            IRiskVehiclePremiumRepository riskVehiclePremiumRepository,
            IRiskPolicyPremiumRepository riskPolicyPremiumRepository
        )
        {
            _vehicleRepository = vehicleRepository;
            _vehiclePremiumRepository = vehiclePremiumRepository;
            _riskDetailRepository = riskDetailRepository;
            _riskManuscriptsRepository = riskManuscriptsRepository;
            _policyHistoryRepository = policyHistoryRepository;
            _bindingRepository = bindingRepository;
            _riskFormRepository = riskFormRepository;
            _billingService = billingService;
            _riskVehiclePremiumRepository = riskVehiclePremiumRepository;
            _riskPolicyPremiumRepository = riskPolicyPremiumRepository;
        }

        public async Task<IAmount> CreateGeneralLiabilityAsync(RiskDetail riskDetail, RiskPolicyPremiumCollection riskPolicyPremiums, DateRange policyPeriod)
        {
            var amountInfo = new AmountInfo(riskDetail.Id, AmountKind.GeneralLiability, "General Liability");
            RiskPolicyPremium submissionRiskPolicyPremium = riskPolicyPremiums.Last();

            if (submissionRiskPolicyPremium.CoveragePremium.Premium.Annual == 0)
                submissionRiskPolicyPremium = null;

            var amount = new PolicyProratedAmount(
                            riskPolicyPremiums,
                            amountInfo,
                            policyPeriod,
                            submissionRiskPolicyPremium);

            return await Task.FromResult(amount);
        }

        public async Task<IAmount> CreatePremiumAmountAsync(RiskDetail riskDetail, RiskPolicyPremiumCollection riskPolicyPremiums, DateRange policyPeriod)
        {
            AmountInfo amountInfo = null;

            RiskPolicyPremium submissionRiskPolicyPremium = riskPolicyPremiums.Last();

            switch (submissionRiskPolicyPremium.PremiumType)
            {
                case "GL":
                    amountInfo = new AmountInfo(riskDetail.Id, AmountKind.GeneralLiability, "General Liability");
                    break;
                case "TI":
                    amountInfo = new AmountInfo(riskDetail.Id, AmountKind.TrailerInterchange, "Trailer Interchange");
                    break;
                case "NO":
                    amountInfo = new AmountInfo(riskDetail.Id, AmountKind.NonOwnedTrailer, "Non-Owned Trailer");
                    break;
            }

            if (submissionRiskPolicyPremium.CoveragePremium.Premium.Annual == 0)
                submissionRiskPolicyPremium = null;

            var amount = new PolicyProratedAmount(
                            riskPolicyPremiums,
                            amountInfo,
                            policyPeriod,
                            submissionRiskPolicyPremium);

            return await Task.FromResult(amount);
        }

        public async Task<IEnumerable<IAmount>> CreateVehiclePremiums(RiskDetail riskDetail, Vehicle vehicle, DateRange policyPeriod)
        {
            var amounts = new List<IAmount>();

            RiskVehiclePremiumCollection riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByVehicleByRiskAsync(riskDetail.RiskId, riskDetail.Id, vehicle.MainId);
            bool isFromSubmission = await _vehicleRepository.IsFromSubmission(vehicle.Id);
            RiskVehiclePremium submissionRiskVehiclePremium = null;

            if (isFromSubmission)
            {
                submissionRiskVehiclePremium = riskVehiclePremiums.First(x => x.Order == 1);
            }

            // AL
            amounts.Add(new VehicleProratedAmount(
                riskVehiclePremiums,
                info: new AmountInfo(vehicle.Id, AmountKind.VehicleAL, vehicle.VIN),
                policyPeriod,
                submissionRiskVehiclePremium)
            );

            // PD Comp FT
            amounts.Add(new VehicleProratedAmount(
                riskVehiclePremiums,
                info: new AmountInfo(vehicle.Id, AmountKind.VehiclePDComprehensive, vehicle.VIN),
                policyPeriod,
                submissionRiskVehiclePremium)
            );

            // PD Collision
            amounts.Add(new VehicleProratedAmount(
                riskVehiclePremiums,
                info: new AmountInfo(vehicle.Id, AmountKind.VehiclePDCollision, vehicle.VIN),
                policyPeriod,
                submissionRiskVehiclePremium)
            );

            // Cargo
            amounts.Add(new VehicleProratedAmount(
                riskVehiclePremiums,
                info: new AmountInfo(vehicle.Id, AmountKind.VehicleCargo, vehicle.VIN),
                policyPeriod,
                submissionRiskVehiclePremium)
            );

            return amounts;
        }

        public async Task<IAmount> CreateManuscriptAsync(RiskManuscripts manuscript, DateRange policyPeriod)
        {
            var riskDetail = await _riskDetailRepository.FindAsync(manuscript.RiskDetailId);
            var submission = await _riskDetailRepository.GetSubmissionRiskDetailAsync(riskDetail.RiskId);
            var submissionManuscripts = await _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == submission.Id);
            bool fromSubmission = submissionManuscripts.Any(x => x.CreatedDate.Ticks == manuscript.CreatedDate.Ticks);

            var kind = manuscript.PremiumType == "AL" ? AmountKind.ManuscriptAL : AmountKind.ManuscriptPD;
            if (!manuscript.IsProrate)
            {
                return new FullyEarnedAmount(new AmountInfo(manuscript.CreatedDate.Ticks, kind, manuscript.Description), 
                    manuscript.Premium, 
                    fromSubmission,
                    manuscript.DeletedDate.HasValue);
            }

            var premiumEffectiveDate = _riskManuscriptsRepository.GetEndorsementEffectiveDate(manuscript);

            var effectivity = new DateRange(premiumEffectiveDate, manuscript.ExpirationDate);
            var period = new ProratedAmountPeriod(manuscript.CreatedDate.Ticks, manuscript.Premium, effectivity, policyPeriod, fromSubmission);
            return ProratedAmount.CreateWithSinglePeriods(
                new AmountInfo(manuscript.CreatedDate.Ticks, kind, manuscript.Title),
                period, policyPeriod);
        }


        public async Task<IAmount> CreateVehicleAL(RiskDetail riskDetail, Vehicle vehicle, DateRange policyPeriod)
        {
            RiskVehiclePremiumCollection riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByVehicleByRiskAsync(riskDetail.RiskId, riskDetail.Id, vehicle.MainId);
            bool isFromSubmission = await _vehicleRepository.IsFromSubmission(vehicle.Id);
            RiskVehiclePremium submissionRiskVehiclePremium = null;
            var amountInfo = new AmountInfo(vehicle.Id, AmountKind.VehicleAL, vehicle.VIN);

            if (isFromSubmission)
            {
                submissionRiskVehiclePremium = riskVehiclePremiums.First();
            }

            var vehicleProratedAmount = new VehicleProratedAmount(
                                            riskVehiclePremiums,
                                            amountInfo,
                                            policyPeriod,
                                            submissionRiskVehiclePremium);
            return vehicleProratedAmount;
        }

        public async Task<IAmount> CreateVehicleCargo(RiskDetail riskDetail, Vehicle vehicle, DateRange policyPeriod)
        {
            RiskVehiclePremiumCollection riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByVehicleByRiskAsync(riskDetail.RiskId, riskDetail.Id, vehicle.MainId);
            bool isFromSubmission = await _vehicleRepository.IsFromSubmission(vehicle.Id);
            RiskVehiclePremium submissionRiskVehiclePremium = null;
            var amountInfo = new AmountInfo(vehicle.Id, AmountKind.VehicleCargo, vehicle.VIN);

            if (isFromSubmission)
            {
                submissionRiskVehiclePremium = riskVehiclePremiums.First();
            }

            var vehicleProratedAmount = new VehicleProratedAmount(
                                            riskVehiclePremiums,
                                            amountInfo,
                                            policyPeriod,
                                            submissionRiskVehiclePremium);
            return vehicleProratedAmount;
        }

        public async Task<IAmount> CreateVehiclePD(RiskDetail riskDetail, Vehicle vehicle, DateRange policyPeriod)
        {
            RiskVehiclePremiumCollection riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByVehicleByRiskAsync(riskDetail.RiskId, riskDetail.Id, vehicle.MainId);
            bool isFromSubmission = await _vehicleRepository.IsFromSubmission(vehicle.Id);
            RiskVehiclePremium submissionRiskVehiclePremium = null;
            var amountInfo = new AmountInfo(vehicle.Id, AmountKind.VehiclePD, vehicle.VIN);

            if (isFromSubmission)
            {
                submissionRiskVehiclePremium = riskVehiclePremiums.First();
            }

            var vehicleProratedAmount = new VehicleProratedAmount(
                                            riskVehiclePremiums,
                                            amountInfo,
                                            policyPeriod,
                                            submissionRiskVehiclePremium);
            return vehicleProratedAmount;
        }


        public async Task<IAmount> CreateVehiclePDCollision(RiskDetail riskDetail, Vehicle vehicle, DateRange policyPeriod)
        {
            RiskVehiclePremiumCollection riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByVehicleByRiskAsync(riskDetail.RiskId, riskDetail.Id, vehicle.MainId);
            bool isFromSubmission = await _vehicleRepository.IsFromSubmission(vehicle.Id);
            RiskVehiclePremium submissionRiskVehiclePremium = null;
            var amountInfo = new AmountInfo(vehicle.Id, AmountKind.VehiclePDCollision, vehicle.VIN);

            if (isFromSubmission)
            {
                submissionRiskVehiclePremium = riskVehiclePremiums.First();
            }

            var vehicleProratedAmount = new VehicleProratedAmount(
                                            riskVehiclePremiums,
                                            amountInfo,
                                            policyPeriod,
                                            submissionRiskVehiclePremium);
            return vehicleProratedAmount;
        }

        public async Task<IAmount> CreateVehiclePDComprehensive(RiskDetail riskDetail, Vehicle vehicle, DateRange policyPeriod)
        {
            RiskVehiclePremiumCollection riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByVehicleByRiskAsync(riskDetail.RiskId, riskDetail.Id, vehicle.MainId);
            bool isFromSubmission = await _vehicleRepository.IsFromSubmission(vehicle.Id);
            RiskVehiclePremium submissionRiskVehiclePremium = null;
            var amountInfo = new AmountInfo(vehicle.Id, AmountKind.VehiclePDComprehensive, vehicle.VIN);

            if (isFromSubmission)
            {
                submissionRiskVehiclePremium = riskVehiclePremiums.First();
            }

            var vehicleProratedAmount = new VehicleProratedAmount(
                                            riskVehiclePremiums,
                                            amountInfo,
                                            policyPeriod,
                                            submissionRiskVehiclePremium);
            return vehicleProratedAmount;
        }



        private async Task<IEnumerable<ProratedAmountPeriod>> CreateVehiclePeriodsAsync(IEnumerable<VehiclePremium> vehiclePremiums, DateRange policyPeriod, AmountKind amountKind)
        {
            if (!(amountKind == AmountKind.VehicleAL
                || amountKind == AmountKind.VehiclePD
                || amountKind == AmountKind.VehicleCargo
                || amountKind == AmountKind.VehiclePDComprehensive
                || amountKind == AmountKind.VehiclePDCollision
            ))
            {
                throw new ArgumentException("Only Vehicle AL, PD, Cargo, Comp, & Collision are allowed.");
            }

            var periods = new LinkedList<ProratedAmountPeriod>();
            vehiclePremiums = vehiclePremiums.OrderByDescending(x => x.CreatedDate);

            VehiclePremium currentVehiclePremium = vehiclePremiums.First();

            DateTime currentEffectiveDate = currentVehiclePremium.EffectiveDate.Value;
            DateTime currentExpirationDate = policyPeriod.End;

            // Iterate backward
            while (currentVehiclePremium != null)
            {
                var policyHistories = await _policyHistoryRepository.GetAsync(x => x.PreviousRiskDetailId == currentVehiclePremium.RiskDetailId.ToString());
                bool policyWasCancelledAndReinstated = policyHistories.Any(x => x.PolicyStatus == RiskStatus.Canceled || x.PolicyStatus == RiskStatus.Reinstated);
                DateTime issuedDateTime = await _policyHistoryRepository.FindAsync(x => x.RiskId == policyHistories.Last().RiskId
                                                                                        && x.EndorsementNumber == "0")
                                                                        .ContinueWith((e) => e.Result.DateCreated.Value);

                bool differentPremiums =
                    (amountKind == AmountKind.VehicleAL && currentVehiclePremium.AL.Annual != currentVehiclePremium.Previous?.AL.Annual)
                    || (amountKind == AmountKind.VehiclePD && currentVehiclePremium.PD.Annual != currentVehiclePremium.Previous?.PD.Annual)
                    || (amountKind == AmountKind.VehicleCargo && currentVehiclePremium.Cargo.Annual != currentVehiclePremium.Previous?.Cargo.Annual)
                    || (amountKind == AmountKind.VehicleCargo && currentVehiclePremium.CompFT.Annual != currentVehiclePremium.Previous?.CompFT.Annual)
                    || (amountKind == AmountKind.VehicleCargo && currentVehiclePremium.Collision.Annual != currentVehiclePremium.Previous?.Collision.Annual);

                if (!policyWasCancelledAndReinstated && (differentPremiums || currentVehiclePremium.Previous is null))
                {
                    var effectivity = new DateRange(currentEffectiveDate, currentExpirationDate);
                    bool isFromSubmission = currentVehiclePremium.CreatedDate <= issuedDateTime && currentVehiclePremium.Previous is null; // all vehicle premiums created beyond issuance date will be tagged as from Endorsement

                    var annualAmount = amountKind switch
                    {
                        AmountKind.VehicleAL => currentVehiclePremium.AL.Annual,
                        AmountKind.VehiclePD => currentVehiclePremium.PD.Annual,
                        AmountKind.VehicleCargo => currentVehiclePremium.Cargo.Annual,
                        AmountKind.VehiclePDComprehensive => currentVehiclePremium.CompFT.Annual,
                        AmountKind.VehiclePDCollision => currentVehiclePremium.Collision.Annual,
                        _ => 0,
                    };

                    var period = new ProratedAmountPeriod(currentVehiclePremium.Id, annualAmount,
                                    effectivity, policyPeriod,
                                    fromSubmission: isFromSubmission);
                    periods.AddLast(period);
                    currentExpirationDate = currentEffectiveDate;
                }

                currentEffectiveDate = currentVehiclePremium?.Previous?.EffectiveDate ?? currentExpirationDate;
                currentVehiclePremium = currentVehiclePremium.Previous != null ?
                    vehiclePremiums.FirstOrDefault(x => x.Id == currentVehiclePremium.Previous.Id) : null;
            }

            return periods;
        }

        public IAmount CreateAdditionalPremium(RiskAdditionalInterest riskAdditionalInterest, AdditionalPremiumType type, bool isBlanket, bool? fromSubmission = null)
        {
            string name = (riskAdditionalInterest.Entity is null) ? "Blanket Coverage" : riskAdditionalInterest.Entity.FullName;
            return type switch
            {
                AdditionalPremiumType.AdditionalInsured => new FullyEarnedAmount(
                    new AmountInfo(riskAdditionalInterest.Id, AmountKind.AdditionalPremiumAI, name),
                    isBlanket ? 500 : 50, fromSubmission, riskAdditionalInterest.RemoveProcessDate.HasValue),
                AdditionalPremiumType.WaiverOfSubrogation => new FullyEarnedAmount(
                    new AmountInfo(riskAdditionalInterest.Id, AmountKind.AdditionalPremiumWOS, name),
                    isBlanket ? 1000 : 250, fromSubmission, riskAdditionalInterest.RemoveProcessDate.HasValue),
                AdditionalPremiumType.PrimaryNonContributory => new FullyEarnedAmount(
                    new AmountInfo(riskAdditionalInterest.Id, AmountKind.AdditionalPremiumPNC, name),
                    isBlanket ? 1500 : 500, fromSubmission, riskAdditionalInterest.RemoveProcessDate.HasValue),
                _ => throw new ArgumentException($"Cannot create additional premium type of '{type}."),
            };
        }

        public async Task<IEnumerable<IAmount>> CreatePDPremiumsAsync(RiskDetail riskDetail)
        {
            var binding = await _bindingRepository.FindAsync(x => x.RiskId == riskDetail.RiskId);
            var riskForms = await _riskFormRepository.GetAsync(x => x.RiskDetailId == riskDetail.Id);
            var riskCov = riskDetail.RiskCoverages.First(x => x.OptionNumber == binding.BindOptionId);
            var riskCovPrem = riskCov.RiskCoveragePremium;
            bool hasHiredPhysicalDamage = riskForms.Any(x => x.FormId == "HiredPhysicalDamage" && (x.IsSelected ?? false));
            bool hasTrailerInterchange = riskForms.Any(x => x.FormId == "TrailerInterchangeCoverage" && (x.IsSelected ?? false));

            var submission = await _riskDetailRepository.GetSubmissionRiskDetailAsync(riskDetail.RiskId);
            var riskFormSubmission = (await _riskFormRepository.GetAsync(x => x.RiskDetailId == submission.Id));
            var riskCovSub = submission.RiskCoverages.First(x => x.OptionNumber == binding.BindOptionId);
            var riskCovSubPrem = riskCovSub.RiskCoveragePremium;

            bool hasHPDsubmission = riskFormSubmission.Any(x => x.FormId == "HiredPhysicalDamage" && (x.IsSelected ?? false));
            bool hasTIEsubmission = riskFormSubmission.Any(x => x.FormId == "TrailerInterchangeCoverage" && (x.IsSelected ?? false));

            var amounts = new List<IAmount>();

            var hiredPd = riskCovPrem.HiredPhysicalDamage;
            var hiredPdSub = riskCovSubPrem.HiredPhysicalDamage;
            if ((hiredPd ?? 0) > 0 && hasHiredPhysicalDamage)
            {
                amounts.Add(new FullyEarnedAmount(new AmountInfo(riskDetail.Id, AmountKind.PDHired, "Hired PD"), hiredPd.Value, hasHPDsubmission));
            }
            else if ((hiredPdSub ?? 0) > 0 && !hasHiredPhysicalDamage && hasHPDsubmission)
            {
                
                amounts.Add(new FullyEarnedAmount(new AmountInfo(riskDetail.Id, AmountKind.PDHired, "Hired PD"), hiredPdSub.Value, hasHPDsubmission, true));
            }

            var trailerInterchange = riskCovPrem.TrailerInterchange;
            var trailerInterchangeSub = riskCovSubPrem.TrailerInterchange;
            if ((trailerInterchange ?? 0) > 0 && hasTrailerInterchange)
            {
                amounts.Add(new FullyEarnedAmount(new AmountInfo(riskDetail.Id, AmountKind.PDTrailer, "Trailer Interchange"), trailerInterchange.Value, hasTIEsubmission));
            }
            else if ((trailerInterchangeSub ?? 0) > 0 && !hasTrailerInterchange && hasTIEsubmission)
            {
                var tieForm = riskFormSubmission.FirstOrDefault(x => x.FormId == "TrailerInterchangeCoverage" && (x.IsSelected ?? false));
                amounts.Add(new FullyEarnedAmount(new AmountInfo(riskDetail.Id, AmountKind.PDTrailer, "Trailer Interchange"), trailerInterchangeSub.Value, hasTIEsubmission, true));
            }

            return amounts;
        }

        public async Task<IEnumerable<IAmount>> CreateVehicleFees(Vehicle vehicle, RiskDetail riskDetail)
        {
            var amounts = new List<IAmount>();
            var policyHistories = await _policyHistoryRepository.GetAsync(x => x.RiskId == riskDetail.RiskId.ToString() && x.PolicyStatus == RiskStatus.Active);
            policyHistories = policyHistories.OrderByDescending(x => x.DateCreated);
            var lastActiveRiskDetail = policyHistories.First();

            var lastIssuedRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(Guid.Parse(lastActiveRiskDetail.PreviousRiskDetailId));
            var riskCovPrem = lastIssuedRiskDetail.RiskCoverages.First().RiskCoveragePremium;

            decimal riskManagementFeeAL = (riskCovPrem.RiskMgrFeeAL ?? 0) / lastIssuedRiskDetail.Vehicles.Count;
            var sfAL = new FullyEarnedAmount(new AmountInfo(vehicle.Id, AmountKind.ServiceFeeAL, vehicle.VIN), riskManagementFeeAL);
            amounts.Add(sfAL);

            decimal riskManagementFeePD = (riskCovPrem.RiskMgrFeePD ?? 0) / lastIssuedRiskDetail.Vehicles.Count;
            var sfPD = new FullyEarnedAmount(new AmountInfo(vehicle.Id, AmountKind.ServiceFeePD, vehicle.VIN), riskManagementFeePD);
            amounts.Add(sfPD);

            return amounts;
        }


    }
}
