using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Policy;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Billing;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Billing.Fees;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Policy
{
    public class ReinstatementService : IReinstatementService
    {
        private readonly IPolicyHistoryRepository _policyHistoryRepository;
        private readonly ICancellationBreakdownRepository _cancellationBreakdownRepository;
        private readonly IReinstatementBreakdownRepository _reinstatementBreakdownRepository;
        private readonly IRiskService _riskService;
        private readonly IFeeRepository _feeRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IEntityAddressRepository _entityAddressRepository;
        private readonly IRiskRepository _riskRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IVehiclePremiumRepository _vehiclePremiumRepository;

        public ReinstatementService(
            IRiskService riskService,
            IPolicyHistoryRepository policyHistoryRepository,
            ICancellationBreakdownRepository cancellationBreakdownRepository,
            IReinstatementBreakdownRepository reinstatementBreakdownRepository,
            IFeeRepository feeRepository,
            IRiskDetailRepository riskDetailRepository,
            IVehicleRepository vehicleRepository,
            IVehiclePremiumRepository vehiclePremiumRepository,
            IEntityAddressRepository entityAddressRepository,
            IRiskRepository riskRepository
        )
        {
            _riskService = riskService;
            _policyHistoryRepository = policyHistoryRepository;
            _cancellationBreakdownRepository = cancellationBreakdownRepository;
            _reinstatementBreakdownRepository = reinstatementBreakdownRepository;
            _feeRepository = feeRepository;
            _riskDetailRepository = riskDetailRepository;
            _entityAddressRepository = entityAddressRepository;
            _riskRepository = riskRepository;
            _vehicleRepository = vehicleRepository;
            _vehiclePremiumRepository = vehiclePremiumRepository;
        }

        public async Task<IEnumerable<ReinstatementBreakdown>> GetLatestAmountBreakdownsByRisk(Guid riskId)
        {
            var risk = await _riskRepository.GetByIdNoTrackingAsync(riskId);
            var policyHistory = await _policyHistoryRepository.GetAsync(x => x.RiskId == riskId.ToString());
            return _reinstatementBreakdownRepository.GetByRiskDetailId(risk.LastReinstatedRiskDetailId.Value);
        }

        public async Task ReverseCancellationAmounts(Guid riskId, Guid cancelledRiskDetailId)
        {
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(cancelledRiskDetailId);
            var policyHistory = await _policyHistoryRepository.GetAsync(x => x.RiskId == riskId.ToString());
            var cancelledPolicyHistory = policyHistory.OrderByDescending(x => x.DateCreated).FirstOrDefault(x => x.PolicyStatus == RiskStatus.Canceled);
            var cancellationBreakdowns = _cancellationBreakdownRepository.GetByRiskDetailId(Guid.Parse(cancelledPolicyHistory.PreviousRiskDetailId));
            var riskDTO = await _riskService.GetByIdIncludeAsync(cancelledRiskDetailId);
            var feeDetails = _feeRepository.GetFeeDetailsByState(riskDTO?.NameAndAddress?.State);

            // Remove all stored breakdowns with same riskdetail
            var storedCancellationBreakdowns = _reinstatementBreakdownRepository.GetByRiskDetailId(cancelledRiskDetailId);
            foreach (var breakdown in storedCancellationBreakdowns)
            {
                _reinstatementBreakdownRepository.Remove(breakdown);
            }

            foreach (var cancellationBreakdown in cancellationBreakdowns)
            {
                var reinstatemntBreakdown = ReinstatementBreakdown.FromCancellation(cancellationBreakdown, cancelledRiskDetailId);
                await _reinstatementBreakdownRepository.AddAsync(reinstatemntBreakdown);
            }

            var reinstatedHistoryCount = policyHistory.Count(x => x.PolicyStatus == RiskStatus.Reinstated);
            var reinstatementAmount = await ReinstatementFeeAmountAsync(riskDetail, reinstatedHistoryCount);
            if (reinstatementAmount > 0)
            {
                var reinstatementFee = new ReinstatementBreakdown
                {
                    RiskDetailId = cancelledRiskDetailId,
                    AmountSubTypeId = FeeKind.ReinstatementFee.Id,
                    Amount = reinstatementAmount,
                    Description = FeeKind.ReinstatementFee.Description
                };
                await _reinstatementBreakdownRepository.AddAsync(reinstatementFee);
            }

            await _reinstatementBreakdownRepository.SaveChangesAsync();

            //PolicyHistory previousActiveHistory = policyHistory.OrderByDescending(x => x.DateCreated).First(x => x.PolicyStatus == RiskStatus.Active);
            //IEnumerable<Vehicle> previousActiveVehicles = await _vehicleRepository.GetByRiskDetailAsync(Guid.Parse(previousActiveHistory.PreviousRiskDetailId));

            //foreach(var vehicle in previousActiveVehicles)
            //{
            //    var vehiclePremium = await _vehiclePremiumRepository.GetByRiskDetailVehicleIdAsync(
            //        Guid.Parse(previousActiveHistory.PreviousRiskDetailId), vehicle.Id);
            //    if(vehiclePremium != null)
            //    {
            //        var clonedVehiclePremium = vehiclePremium.Clone();
            //        clonedVehiclePremium.RiskDetailId = reinstatedRiskDetailId;
            //        clonedVehiclePremium.VehicleId = vehicle.Id;
            //        //clonedVehiclePremium.ChangePrevious(vehiclePremium);
            //        await _vehiclePremiumRepository.AddAsync(clonedVehiclePremium);
            //        await _vehiclePremiumRepository.SaveChangesAsync();
            //    }
            //}
        }

        private async Task<decimal> ReinstatementFeeAmountAsync(RiskDetail riskDetail, int reinstatedHistoryCount)
        {
            try
            {
                var businessAddress = await _entityAddressRepository
                    .FindAsync(x => x.AddressTypeId == (short)AddressTypesEnum.Business
                    && x.EntityId == riskDetail.Insured.EntityId, i => i.Address);

                var feeDetails = _feeRepository.GetFeeDetailsByState(businessAddress.Address.State);

                switch (businessAddress.Address.State)
                {
                    case "CA":
                    case "NJ":
                    case "GA":
                    case "MO":
                    case "PA":
                    case "TX":
                    case "FL":
                    case "CO":
                    case "OH":
                    case "VA":
                    case "TN":
                    case "IL":
                    case "AZ":
                    case "MS":
                    case "SC":
                        if (reinstatedHistoryCount <= 2)
                        {
                            return feeDetails.Where(x => x.FeeKindId == FeeKind.ReinstatementFee.Id).Select(s => s.Amount).FirstOrDefault();
                        }
                        else if (reinstatedHistoryCount > 2 && reinstatedHistoryCount % 2 == 0) // Every other time after 2nd
                        {
                            return feeDetails.Where(x => x.FeeKindId == FeeKind.ReinstatementFee2.Id).Select(s => s.Amount).FirstOrDefault();
                        }
                        break;
                    case "IN":
                    case "OR":
                    case "AL":
                        return feeDetails.Where(x => x.FeeKindId == FeeKind.ReinstatementFee.Id).Select(s => s.Amount).FirstOrDefault();
                }
                return 0;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

    }
}
