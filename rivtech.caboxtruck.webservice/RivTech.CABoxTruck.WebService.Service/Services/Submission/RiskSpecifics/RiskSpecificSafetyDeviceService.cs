using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics.MaintenanceSafety;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Submission.RiskSpecifics
{
    public class RiskSpecificSafetyDeviceService : IRiskSpecificSafetyDeviceService
    {
        private readonly IRiskSpecificSafetyDeviceRepository _riskSpecificSafetyDeviceRepository;
        private readonly IMapper _mapper;

        public RiskSpecificSafetyDeviceService(
            IRiskSpecificSafetyDeviceRepository riskSpecificSafetyDeviceRepository,
            IMapper mapper
        )
        {
            _riskSpecificSafetyDeviceRepository = riskSpecificSafetyDeviceRepository;
            _mapper = mapper;
        }

        public async Task<List<RiskSpecificSafetyDeviceDTO>> GetRiskSpecificSafetyDevice(Guid riskId)
        {
            var safetyDevice = await _riskSpecificSafetyDeviceRepository.GetAsync(x => x.RiskDetailId == riskId);
            return _mapper.Map<List<RiskSpecificSafetyDeviceDTO>>(safetyDevice);
        }

        public async Task<RiskSpecificSafetyDeviceDTO> InsertRiskSpecificSafetyDevice(RiskSpecificSafetyDeviceDTO safetyDevice)
        {
            var result = await _riskSpecificSafetyDeviceRepository.AddAsync(_mapper.Map<RiskSpecificSafetyDevice>(safetyDevice));
            await _riskSpecificSafetyDeviceRepository.SaveChangesAsync();
            return _mapper.Map<RiskSpecificSafetyDeviceDTO>(result);
        }

        public async Task<RiskSpecificSafetyDeviceDTO> UpdateSpecificSafetyDevice(RiskSpecificSafetyDeviceDTO safetyDevice, Guid riskId)
        {
            var result = _riskSpecificSafetyDeviceRepository.Update(_mapper.Map<RiskSpecificSafetyDevice>(safetyDevice));
            await _riskSpecificSafetyDeviceRepository.SaveChangesAsync();
            return _mapper.Map<RiskSpecificSafetyDeviceDTO>(result);
        }

        public async void DeleteSpecificSafetyDevice(RiskSpecificSafetyDeviceDTO safetyDevice)
        {
            _riskSpecificSafetyDeviceRepository.Remove(_mapper.Map<RiskSpecificSafetyDevice>(safetyDevice));
            await _riskSpecificSafetyDeviceRepository.SaveChangesAsync();
        }

        public async Task<List<RiskSpecificSafetyDeviceDTO>> SaveRiskSpecificSafetyDevice(List<RiskSpecificSafetyDeviceDTO> safetyDevices, Guid riskId)
        {
            List<RiskSpecificSafetyDeviceDTO> safetyDeviceResults = new List<RiskSpecificSafetyDeviceDTO>();
            var existingSafetyDevices = await _riskSpecificSafetyDeviceRepository.GetAsync(x => x.RiskDetailId == riskId);
            var dataToRemove = existingSafetyDevices.ToList();

            foreach (var safetyDevice in safetyDevices)
            {
                safetyDevice.RiskDetailId = riskId;
                if (string.IsNullOrEmpty(safetyDevice.UnitDescription)) safetyDevice.UnitDescription = null;
                   
                var existingData = existingSafetyDevices.FirstOrDefault(x => x.SafetyDeviceCategoryId == safetyDevice.SafetyDeviceCategoryId);                
                
                if (existingData == null)
                {
                    safetyDeviceResults.Add(await InsertRiskSpecificSafetyDevice(safetyDevice));
                }
                else 
                {
                    safetyDevice.Id = existingData.Id;
                    safetyDeviceResults.Add(await UpdateSpecificSafetyDevice(safetyDevice, riskId));
                    dataToRemove.Remove(dataToRemove.FirstOrDefault(x => x.Id == safetyDevice.Id));
                }
            }

            foreach(var data in dataToRemove) 
            {
                _riskSpecificSafetyDeviceRepository.Remove(data);
            }
            await _riskSpecificSafetyDeviceRepository.SaveChangesAsync();
            return safetyDeviceResults;
        }
    }
}
