using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Submission.RiskSpecifics
{
    public class RiskSpecificDriverInfoPageService : IRiskSpecificDriverInfoPageService
    {
        private readonly IRiskSpecificDriverInfoRepository _riskSpecificDriverInfoRepository;
        private readonly IRiskSpecificDriverHiringCriteriaRepository _riskSpecificDriverHiringCriteriaRepository;
        private readonly IMvrPullingFrequencyOptionRepository _mvrPullingFrequencyOptionRepository;
        private readonly IBackgroundCheckOptionRepository _backgroundCheckOptionRepository;
        private readonly IDriverTrainingOptionRepository _driverTrainingOptionRepository;
        private readonly IRiskDetailRepository _riskRepository;
        private readonly IMapper _mapper;

        public RiskSpecificDriverInfoPageService(IRiskSpecificDriverInfoRepository riskSpecificDriverInfoRepository,
            IRiskSpecificDriverHiringCriteriaRepository riskSpecificDriverHiringCriteriaRepository,
            IDriverTrainingOptionRepository driverTrainingOptionRepository,
            IBackgroundCheckOptionRepository backgroundCheckOptionRepository,
            IRiskDetailRepository riskRepository, IMvrPullingFrequencyOptionRepository mvrPullingFrequencyOptionRepository, IMapper mapper)
        {
            _riskSpecificDriverInfoRepository = riskSpecificDriverInfoRepository;
            _riskSpecificDriverHiringCriteriaRepository = riskSpecificDriverHiringCriteriaRepository;
            _mvrPullingFrequencyOptionRepository = mvrPullingFrequencyOptionRepository;
            _backgroundCheckOptionRepository = backgroundCheckOptionRepository;
            _driverTrainingOptionRepository = driverTrainingOptionRepository;
            _riskRepository = riskRepository;
            _mapper = mapper;
        }

        public async Task<string> UpdateDriverInfoAndHiringCriteriaAsync(Guid riskId, RiskSpecificDriverInfoPageDto model)
        {
            if (model?.DriverInfo != null)
            {
                var driverInfoFromDb = await _riskSpecificDriverInfoRepository.FindAsync(d => d.Id == riskId, d => d.MvrPullingFrequency);
                if (driverInfoFromDb != null)
                {
                    if (driverInfoFromDb.MvrPullingFrequency?.Id != model.DriverInfo.MvrPullingFrequency?.Id)
                    {
                        driverInfoFromDb.MvrPullingFrequency = null;
                    }
                    model.DriverInfo.Id = driverInfoFromDb.Id;
                    _mapper.Map(model.DriverInfo, driverInfoFromDb);
                    driverInfoFromDb.IsActive = true;
                    await _riskSpecificDriverInfoRepository.SaveChangesAsync();
                }
                else
                {
                    var risk = await _riskRepository.FindAsync(riskId);
                    if (risk == null)
                    {
                        throw new ModelNotFoundException("Risk data not found");
                    }

                    var newDriverInfo = _mapper.Map<RiskSpecificDriverInfo>(model.DriverInfo);
                    newDriverInfo.IsActive = true;
                    if (newDriverInfo.MvrPullingFrequency != null)
                    {
                        var mvrPullingFrequencyFromDb =
                            await _mvrPullingFrequencyOptionRepository.FindAsync(newDriverInfo.MvrPullingFrequency.Id);
                        if (mvrPullingFrequencyFromDb != null)
                        {
                            newDriverInfo.MvrPullingFrequency = mvrPullingFrequencyFromDb;
                        }
                    }

                    risk.DriverInfo = newDriverInfo;
                    await _riskRepository.SaveChangesAsync();
                }
            }

            if (model?.DriverHiringCriteria != null)
            {
                var driverHiringCriteriaFromDb = await _riskSpecificDriverHiringCriteriaRepository.FindAsync(riskId);
                if (driverHiringCriteriaFromDb != null)
                {
                    var linkedBackgroundCheckOptions = await _riskSpecificDriverHiringCriteriaRepository.GetBackgroundCheckOptions(riskId);
                    linkedBackgroundCheckOptions.RemoveAll(x => true);

                    var linkedDriverTrainingOptions = await _riskSpecificDriverHiringCriteriaRepository.GetDriverTrainingOptions(riskId);
                    linkedDriverTrainingOptions.RemoveAll(x => true);
                    await _riskSpecificDriverHiringCriteriaRepository.SaveChangesAsync();

                    model.DriverHiringCriteria.Id = driverHiringCriteriaFromDb.Id;
                    _mapper.Map(model.DriverHiringCriteria, driverHiringCriteriaFromDb);
                    driverHiringCriteriaFromDb.IsActive = true;

                    driverHiringCriteriaFromDb.BackGroundCheckIncludes.Clear();
                    driverHiringCriteriaFromDb.DriverTrainingIncludes.Clear();

                    if (model.DriverHiringCriteria.BackGroundCheckIncludes != null && model.DriverHiringCriteria.BackGroundCheckIncludes.Count > 0)
                    {
                        foreach (var option in model.DriverHiringCriteria.BackGroundCheckIncludes)
                        {
                            var optionFromDb = await _backgroundCheckOptionRepository.FindAsync(option.Id);
                            if (optionFromDb != null)
                            {
                                driverHiringCriteriaFromDb.BackGroundCheckIncludes.Add(optionFromDb);
                            }
                        }
                    }

                    if (model.DriverHiringCriteria.DriverTrainingIncludes != null && model.DriverHiringCriteria.DriverTrainingIncludes.Count > 0)
                    {
                        foreach (var option in model.DriverHiringCriteria.DriverTrainingIncludes)
                        {
                            var optionFromDb = await _driverTrainingOptionRepository.FindAsync(option.Id);
                            if (optionFromDb != null)
                            {
                                driverHiringCriteriaFromDb.DriverTrainingIncludes.Add(optionFromDb);
                            }
                        }
                    }
                    await _driverTrainingOptionRepository.SaveChangesAsync();
                }
                else
                {
                    var risk = await _riskRepository.FindAsync(riskId);
                    if (risk == null)
                    {
                        throw new ModelNotFoundException("Risk data not found");
                    }

                    risk.DriverHiringCriteria = _mapper.Map<RiskSpecificDriverHiringCriteria>(model.DriverHiringCriteria);
                    risk.DriverHiringCriteria.IsActive = true;

                    risk.DriverHiringCriteria.DriverTrainingIncludes.RemoveAll(x => true);
                    risk.DriverHiringCriteria.BackGroundCheckIncludes.RemoveAll(x => true);
                    await _riskRepository.SaveChangesAsync();
                    if (model.DriverHiringCriteria.BackGroundCheckIncludes != null && model.DriverHiringCriteria.BackGroundCheckIncludes.Count > 0)
                    {
                        foreach (var option in model.DriverHiringCriteria.BackGroundCheckIncludes)
                        {
                            var optionFromDb = await _backgroundCheckOptionRepository.FindAsync(option.Id);
                            if (optionFromDb != null)
                            {
                                risk.DriverHiringCriteria.BackGroundCheckIncludes.Add(optionFromDb);
                            }
                        }
                    }

                    if (model.DriverHiringCriteria.DriverTrainingIncludes != null && model.DriverHiringCriteria.DriverTrainingIncludes.Count > 0)
                    {
                        foreach (var option in model.DriverHiringCriteria.DriverTrainingIncludes)
                        {
                            var optionFromDb = await _driverTrainingOptionRepository.FindAsync(option.Id);
                            if (optionFromDb != null)
                            {
                                risk.DriverHiringCriteria.DriverTrainingIncludes.Add(optionFromDb);
                            }
                        }
                    }

                    await _riskRepository.SaveChangesAsync();
                }
            }
            return riskId.ToString();
        }
    }
}
