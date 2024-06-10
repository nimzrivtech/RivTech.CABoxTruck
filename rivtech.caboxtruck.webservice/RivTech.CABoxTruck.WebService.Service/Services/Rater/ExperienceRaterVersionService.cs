using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.DTO.Rater;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Rater.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RiskEntity = RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater
{
    public class MismatchExperienceRater
    {
        public string SubmissionNumber { get; set; }
        public Guid RiskDetailId { get; internal set; }
        public int RaterVersionId { get; set; }
        public int XPRaterVersionId { get; set; }
        public string Status { get; internal set; }
    }

    public interface IExperienceRaterVersionService
    {
        Task ConfirmVersionUpdateAsync(Guid riskId);
        Task<List<MismatchExperienceRater>> GetRisksWithMismatchAsync();
        Task UpdateExperienceRaterAsync(Guid riskDetailId);
    }

    public class ExperienceRaterVersionService : IExperienceRaterVersionService
    {
        private readonly ILogger<ExperienceRaterVersionService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRiskRepository _riskRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IRiskService _riskService;
        private readonly IRaterInputValuesFactory _raterInputValuesFactory;
        private readonly IRiskCoverageRepository _riskCoverageRepository;
        private readonly AppDbContext _dbContext;

        public ExperienceRaterVersionService(
            ILogger<ExperienceRaterVersionService> logger,
            IHttpClientFactory httpClientFactory,
            IRiskRepository riskRepository,
            IRiskDetailRepository riskDetailRepository,
            IRiskService riskService ,
            IRaterInputValuesFactory raterInputValuesFactory,
            IRiskCoverageRepository riskCoverageRepository,
            AppDbContext dbContext
        )
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _riskRepository = riskRepository;
            _riskDetailRepository = riskDetailRepository;
            _riskService = riskService;
            _raterInputValuesFactory = raterInputValuesFactory;
            _riskCoverageRepository = riskCoverageRepository;
            _dbContext = dbContext;
        }

        public async Task<List<MismatchExperienceRater>> GetRisksWithMismatchAsync()
        {
            using var httpClient = _httpClientFactory.CreateClient("rater");
            string url = $"api/ExperienceRater/list";
            var preloadResponse = await httpClient.GetAsync(url);

            var result = await preloadResponse.Content.ReadAsStringAsync();
            if (!preloadResponse.IsSuccessStatusCode)
            {
                if (preloadResponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    result = "Error occurred from rater server";
                }
                throw new RaterException(preloadResponse.ReasonPhrase);
            }

            var xpRaters = JsonConvert.DeserializeObject<ExperienceRaterDTO[]>(result);
            var unquotedRisks = await GetUnquotedRisksAsync();
            var mismtachList = new List<MismatchExperienceRater>();
            foreach (var risk in unquotedRisks)
            {
                var xpRater = xpRaters.FirstOrDefault(x => x.SubmissionNumber == risk.SubmissionNumber);
                if (xpRater != null && risk.RaterVersionId != xpRater.RaterVersionId)
                    mismtachList.Add(new MismatchExperienceRater
                    {
                        SubmissionNumber = risk.SubmissionNumber,
                        RiskDetailId = risk.RiskDetails.First(x => x.EndorsementNumber == 0).Id,
                        RaterVersionId = risk.RaterVersionId,
                        XPRaterVersionId = xpRater.RaterVersionId,
                        Status = risk.RiskDetails.First().Status
                    });
            }

            return mismtachList;
        }

        public async Task UpdateExperienceRaterAsync(Guid riskDetailId)
        {
            try
            {
                var riskDetail = await _riskDetailRepository.FindAsync(riskDetailId);
                var risk = await _riskRepository.FindAsync(riskDetail.RiskId);

                var riskDTO = await _riskService.GetByIdIncludeAsync(riskDetailId, withAgency: false);
                await ResetExperienceRaterAsync(riskDTO);
                await ResetXPRatingFactors(riskDTO);
                risk.UpdateXPRaterVersion = true;

                _riskRepository.Update(risk);
                await _riskRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured while updating experience rater.");
                throw;
            }
        
        }

        private async Task ResetExperienceRaterAsync(RiskDTO riskDTO)
        {
            string url = $"api/ExperienceRater/Reset/{riskDTO.SubmissionNumber}?raterVersionId={riskDTO.RaterVersionId}";
            var inputData = await _raterInputValuesFactory.CreateAsync(riskDTO, optionNumber: 1);
            var content = new StringContent(JsonConvert.SerializeObject(inputData), Encoding.UTF8, "application/json");
            using var httpClient = _httpClientFactory.CreateClient("rater");
            var response = await httpClient.PutAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new RaterException(response.ReasonPhrase);
            }
        }

        private async Task ResetXPRatingFactors(RiskDTO riskDTO)
        {
            foreach (var riskCovDto in riskDTO.RiskCoverages)
            {
                var riskCov = await _riskCoverageRepository.FindAsync(riskCovDto.Id);
                riskCov.LiabilityExperienceRatingFactor = 1;
                riskCov.LiabilityScheduleRatingFactor = 1;
                await _riskCoverageRepository.SaveChangesAsync();

                riskCovDto.LiabilityScheduleRatingFactor = riskCov.LiabilityScheduleRatingFactor;
                riskCovDto.LiabilityExperienceRatingFactor = riskCov.LiabilityExperienceRatingFactor;
            }
        }


        private async Task<IEnumerable<RiskEntity>> GetUnquotedRisksAsync()
        {
            var risks = await _dbContext.Set<RiskEntity>().AsNoTracking().AsSplitQuery()
                            .Include(x => x.RiskDetails).AsNoTracking().AsSplitQuery()
                            .Where(x => x.Status == RiskStatus.Pending)
                            .Select(risk => risk)
                            .ToListAsync();

            //var unquotedStatuses = new[]
            //{
            //    RiskDetailStatus.Incomplete,
            //    RiskDetailStatus.InResearch,
            //    RiskDetailStatus.Draft,
            //    RiskDetailStatus.Received,
            //    RiskDetailStatus.Quoted,
            //};

            //return risks.Where(x => unquotedStatuses.Contains(x.RiskDetails.First().Status)).ToList();
            return risks;

        }

        public async Task ConfirmVersionUpdateAsync(Guid riskId)
        {
            var risk = await _riskRepository.FindAsync(riskId);
            risk.UpdateXPRaterVersion = false;
            await _riskRepository.SaveChangesAsync();
        }
    }


}
