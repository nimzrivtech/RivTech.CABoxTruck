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
    public class RiskSpecificDotInfoService : IRiskSpecificDotInfoService
    {
        private readonly IRiskSpecificDotInfoRepository _riskSpecificDotInfoRepository;
        private readonly IRiskDetailRepository _riskRepository;
        private readonly IChameleonIssuesOptionRepository _chameleonIssuesOptionRepository;
        private readonly ISaferRatingOptionRepository _saferRatingOptionRepository;
        private readonly ITrafficLightRatingOptionRepository _trafficLightRatingOptionRepository;
        private readonly IMapper _mapper;

        public RiskSpecificDotInfoService(IRiskSpecificDotInfoRepository riskSpecificDotInfoRepository,
            IRiskDetailRepository riskRepository,
            IChameleonIssuesOptionRepository chameleonIssuesOptionRepository,
            ISaferRatingOptionRepository saferRatingOptionRepository,
            ITrafficLightRatingOptionRepository trafficLightRatingOptionRepository, IMapper mapper)
        {
            _riskRepository = riskRepository;
            _riskSpecificDotInfoRepository = riskSpecificDotInfoRepository;
            _chameleonIssuesOptionRepository = chameleonIssuesOptionRepository;
            _saferRatingOptionRepository = saferRatingOptionRepository;
            _trafficLightRatingOptionRepository = trafficLightRatingOptionRepository;
            _mapper = mapper;
        }

        public async Task<string> RemoveDotInfoAsync(Guid id)
        {
            var entity = await _riskSpecificDotInfoRepository.FindAsync(x => x.Id == id && x.IsActive);
            entity.IsActive = false;
            await _riskSpecificDotInfoRepository.SaveChangesAsync();
            return entity.Id.ToString();
        }

        public async Task<string> UpdateDotInfoAsync(Guid riskId, RiskSpecificDotInfoDto model)
        {
            if (model != null)
            {
                var dotInfoFromDb = await _riskSpecificDotInfoRepository.FindAsync(riskId);
                if (dotInfoFromDb != null)
                {
                    model.Id = dotInfoFromDb.Id;
                    _mapper.Map(model, dotInfoFromDb);
                    dotInfoFromDb.IsActive = true;

                    if (dotInfoFromDb.ChameleonIssues != null)
                    {
                        var chameleonIssuesFromDb = await _chameleonIssuesOptionRepository.FindAsync(dotInfoFromDb.ChameleonIssues.Id);
                        if (chameleonIssuesFromDb != null)
                        {
                            dotInfoFromDb.ChameleonIssues = chameleonIssuesFromDb;
                        }
                    }

                    if (dotInfoFromDb.CurrentSaferRating != null)
                    {
                        var saferRatingFromDb = await _saferRatingOptionRepository.FindAsync(dotInfoFromDb.CurrentSaferRating.Id);
                        if (saferRatingFromDb != null)
                        {
                            dotInfoFromDb.CurrentSaferRating = saferRatingFromDb;
                        }
                    }

                    if (dotInfoFromDb.IssCabRating != null)
                    {
                        var trafficLightRatingFromDb = await _trafficLightRatingOptionRepository.FindAsync(dotInfoFromDb.IssCabRating.Id);
                        if (trafficLightRatingFromDb != null)
                        {
                            dotInfoFromDb.IssCabRating = trafficLightRatingFromDb;
                        }
                    }

                    await _riskSpecificDotInfoRepository.SaveChangesAsync();
                }
                else
                {
                    var risk = await _riskRepository.FindAsync(riskId);
                    if (risk == null)
                    {
                        throw new ModelNotFoundException("Risk data not found");
                    }

                    var newDotInfo = _mapper.Map<RiskSpecificDotInfo>(model);
                    newDotInfo.IsActive = true;

                    if (newDotInfo.ChameleonIssues != null)
                    {
                        var chameleonIssuesFromDb = await _chameleonIssuesOptionRepository.FindAsync(newDotInfo.ChameleonIssues.Id);
                        if (chameleonIssuesFromDb != null)
                        {
                            newDotInfo.ChameleonIssues = chameleonIssuesFromDb;
                        }
                    }

                    if (newDotInfo.CurrentSaferRating != null)
                    {
                        var saferRatingFromDb = await _saferRatingOptionRepository.FindAsync(newDotInfo.CurrentSaferRating.Id);
                        if (saferRatingFromDb != null)
                        {
                            newDotInfo.CurrentSaferRating = saferRatingFromDb;
                        }
                    }

                    if (newDotInfo.IssCabRating != null)
                    {
                        var trafficLightRatingFromDb = await _trafficLightRatingOptionRepository.FindAsync(newDotInfo.IssCabRating.Id);
                        if (trafficLightRatingFromDb != null)
                        {
                            newDotInfo.IssCabRating = trafficLightRatingFromDb;
                        }
                    }

                    risk.DotInfo = newDotInfo;
                    await _riskRepository.SaveChangesAsync();
                }

                return riskId.ToString();
            }
            else
            {
                throw new ArgumentException("Invalid DTO");
            }
        }
    }
}
