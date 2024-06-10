using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class CoverageHistoryService : ICoverageHistoryService
    {
        private readonly IRiskHistoryRepository _riskHistoryRepository;
        private readonly IMapper _mapper;

        public CoverageHistoryService(IRiskHistoryRepository riskHistoryRepository, IMapper mapper)
        {
            _riskHistoryRepository = riskHistoryRepository;
            _mapper = mapper;
        }

        public async Task<RiskHistoryListDTO> GetByIdAsync(Guid id)
        {
            RiskHistoryListDTO returnList = new RiskHistoryListDTO();
            returnList.RiskHistories = new List<RiskHistoryDTO>();

            for (var i = 0; i <= 4; i++)
            {
                var result = await _riskHistoryRepository.FindAsync(x => x.RiskDetailId == id && x.HistoryYear == i);
                if (result != null)
                {
                    returnList.RiskHistories.Add(_mapper.Map<RiskHistoryDTO>(result));
                } else
                {
                    returnList.RiskHistories.Add(new RiskHistoryDTO() { HistoryYear = i });
                }
            }
            returnList.RiskDetailId = id;
            return returnList;
        }
        public async Task<bool> IsExistAsync(Guid id)
        {
            return await _riskHistoryRepository.AnyAsync(x => x.RiskDetailId == id);
        }

        public async Task<RiskHistoryListDTO> InsertAsync(RiskHistoryListDTO riskHistory)
        {
            for (var i = 0; i < riskHistory.RiskHistories.Count; i++)
            {
                riskHistory.RiskHistories[i].RiskDetailId = riskHistory.RiskDetailId;
                riskHistory.RiskHistories[i].HistoryYear = i;
                await _riskHistoryRepository.AddAsync(_mapper.Map<RiskHistory>(riskHistory.RiskHistories[i]));
            }

            await _riskHistoryRepository.SaveChangesAsync();
            return riskHistory;
        }

        public async Task<RiskHistoryListDTO> UpdateAsync(RiskHistoryListDTO riskHistory)
        {
            foreach(var item in riskHistory.RiskHistories)
            {
                var data = await _riskHistoryRepository.FindAsync(x => x.RiskDetailId == riskHistory.RiskDetailId && x.HistoryYear == item.HistoryYear);
                if (data is null)
                    continue;

                data.PolicyTermStartDate = item.PolicyTermStartDate;
                data.PolicyTermEndDate = item.PolicyTermEndDate;
                data.InsuranceCarrierOrBroker = item.InsuranceCarrierOrBroker;
                data.LiabilityLimits = item.LiabilityLimits;
                data.LiabilityDeductible = item.LiabilityDeductible;
                data.CollisionDeductible = item.CollisionDeductible;
                data.ComprehensiveDeductible = item.ComprehensiveDeductible;
                data.CargoLimits = item.CargoLimits;
                data.RefrigeratedCargoLimits = item.RefrigeratedCargoLimits;
                data.AutoLiabilityPremiumPerVehicle = item.AutoLiabilityPremiumPerVehicle;
                data.PhysicalDamagePremiumPerVehicle = item.PhysicalDamagePremiumPerVehicle;
                data.TotalPremiumPerVehicle = item.TotalPremiumPerVehicle;
                data.NumberOfVehicles = item.NumberOfVehicles;
                data.NumberOfPowerUnits = item.NumberOfPowerUnits;
                data.NumberOfSpareVehicles = item.NumberOfSpareVehicles;
                data.NumberOfDrivers = item.NumberOfDrivers;
                data.LossRunDateForEachTerm = item.LossRunDateForEachTerm;
                data.GrossRevenues = item.GrossRevenues;
                data.TotalFleetMileage = item.TotalFleetMileage;
                data.NumberOfOneWayTrips = item.NumberOfOneWayTrips;

                _riskHistoryRepository.Update(data);
            }
            
            await _riskHistoryRepository.SaveChangesAsync();

            return riskHistory;
        }

        public async Task DeleteFieldsAsync(Guid riskId, bool isVehicle = false, bool isDriver = false, bool isComprehensive = false, bool isCollision = false, bool isCargo = false, bool isRef = false)
        {
            var data = _riskHistoryRepository.GetAsync(x => x.RiskDetailId == riskId).Result;
            foreach(var item in data)
            {
                if (isVehicle) item.NumberOfSpareVehicles = null;
                if (isDriver) item.NumberOfDrivers = null;
                if (isCollision) item.CollisionDeductible = null;
                if (isComprehensive) item.ComprehensiveDeductible = null;
                if (isCargo) item.CargoLimits = null;
                if (isRef) item.RefrigeratedCargoLimits = null;
                _riskHistoryRepository.Update(item);
            }

            await _riskHistoryRepository.SaveChangesAsync();
        }

        public async Task UpdatePowerUnitFieldsAsync(RiskHistoryDTO riskHistory)
        {
            var data = await _riskHistoryRepository.FindAsync(x => x.RiskDetailId == riskHistory.RiskDetailId && x.HistoryYear == riskHistory.HistoryYear);

            if (data != null)
            {
                data.NumberOfPowerUnitsAL = riskHistory.NumberOfPowerUnitsAL;
                data.NumberOfPowerUnitsAPD = riskHistory.NumberOfPowerUnitsAPD;

                _riskHistoryRepository.Update(data);
                await _riskHistoryRepository.SaveChangesAsync();
            }
        }
    }
}
