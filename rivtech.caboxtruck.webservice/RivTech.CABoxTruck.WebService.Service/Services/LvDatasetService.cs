using AutoMapper;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.DTO.Rater;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.Service.Services.Common;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class LvDatasetService : ILvDatasetService
    {
        private readonly ILvDatasetRepository _lvDatasetRepository;
        private readonly IMapper _mapper;

        public LvDatasetService(ILvDatasetRepository lvDatasetRepository,IMapper mapper) {
            _lvDatasetRepository = lvDatasetRepository;
            _mapper = mapper;
        }

      

        public async Task<List<LvDatasetDTO>> GetByTypeAndRaterVersionAsync(string type, bool isCurrentlySupported)
        {
            List<LvDatasetDTO> result = new List<LvDatasetDTO>();

            List<LvDatasetDTO> mainGVW = _mapper.Map<List<LvDatasetDTO>>(await _lvDatasetRepository.GetAsync(x => x.IsActive && x.DatasetType == LvDatasetType.MainGVW));
            List<LvDatasetDTO> vehicleGVW = _mapper.Map<List<LvDatasetDTO>>(await _lvDatasetRepository.GetAsync(x => x.IsActive && x.DatasetType == LvDatasetType.VehicleGVW));
            List<LvDatasetDTO> vehicleDesc = _mapper.Map<List<LvDatasetDTO>>(await _lvDatasetRepository.GetAsync(x => x.IsActive && x.DatasetType == LvDatasetType.VehicleDescription));
            List<LvDatasetDTO> secondaryBusGroup = _mapper.Map<List<LvDatasetDTO>>(await _lvDatasetRepository.GetAsync(x => x.IsActive && x.DatasetType == LvDatasetType.SecondaryBusinessGroup));

            result = (string)type switch
            {
                LvDatasetType.MainGVW => _mapper.Map<List<LvDatasetDTO>>(FilterDatasetIfSupported(mainGVW, isCurrentlySupported)),
                LvDatasetType.VehicleGVW => _mapper.Map<List<LvDatasetDTO>>(FilterDatasetIfSupported(vehicleGVW, isCurrentlySupported)),
                LvDatasetType.VehicleDescription => _mapper.Map<List<LvDatasetDTO>>(FilterDatasetIfSupported(vehicleDesc, isCurrentlySupported)),
                LvDatasetType.SecondaryBusinessGroup => _mapper.Map<List<LvDatasetDTO>>(FilterDatasetIfSupported(secondaryBusGroup, isCurrentlySupported)),
                _ => new List<LvDatasetDTO>()
            };

            return result;
        }

        private List<LvDatasetDTO> FilterDatasetIfSupported(List<LvDatasetDTO> list, bool isCurrentlySupported)
        {
            List<LvDatasetDTO> result = new List<LvDatasetDTO>();

            List<string> oldVersions = new List<string> { "1", "2", "3", "4", "5", "6" };
            var currentlySupportedVersion = "7";
            if (isCurrentlySupported)
            {
                // Filter for currently supported version "7"
                result = list.Where(x => x.SupportedInRaterVersion.Split(",").Any(o => o.Trim() == currentlySupportedVersion)).OrderBy(x => x.OrderInUI).ToList();
            }
            else
            {
                // Filter for old versions in the "oldVersions" list
                result = list.Where(x => x.SupportedInRaterVersion.Split(",").Any(o => oldVersions.Contains(o.Trim()))).OrderBy(x => x.OrderInUI).ToList();
            }

            return result;
        }
        public string GetByTypeAndRaterVersionAndValueAsync(string type, bool isCurrentlySupported, int Value)
        {
            LvDatasetDTO data =  _mapper.Map<LvDatasetDTO>(_lvDatasetRepository.FindAsync(x => x.IsActive && x.DatasetType == LvDatasetType.VehicleDescription && x.Value == Value));
            return data.DescriptionInUI.ToString();
        }

    }
}
