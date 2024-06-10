using Newtonsoft.Json.Linq;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Rater;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IRaterService
    {
        Task<dynamic> MapFields(Guid id, JObject requestBody, int optionNo = 1, bool isPremiumMapping = false, bool? fromPolicy = null);

        Task<List<string>> GetOptions(Guid id);

        dynamic ParseListToValue(dynamic dbVal, JToken node);
        Task<bool> CopyRaterAsync(string submissionNumber, string newSubmissionNumber);

        Task DeleteArchive(string submissionNumber, int endorsementNumber);
        Task<string> PreloadPremiumRater(string submissionNumber, Guid riskDetailId, bool? fromPolicy);
        Task CalculatePremiumAsync(Guid riskDetailId, int endorsementNumber, bool fromPolicy, int optionId = 0, bool forIssuance = false);
        Task<string> PreloadPremiumRater(string submissionNumber, int raterVersionId, bool fromPolicy = true);
        Task UnloadPremiumRaterAsync(string submissionNumber);
        Task<string> ExtractAndSaveRatingFactors(Guid guid, string calculatedResults, bool fromPolicy, RiskDTO riskDTO);
        Task UpdateRatingFactors(Guid riskDetailId, RatingFactorsDTO ratingFactors);
        Task<RatingFactorsDTO> GetRatingFactorsAsync(Guid riskDetailId);

        Task<dynamic> GetRaterVersionAsync(DateTime policyEffectiveDate);
        Task<RaterVersionDTO> GetRaterVersionAsync(int raterVersionId, Guid riskDetailId, bool isRaterVersionLocked = false);

        Task CalculatePremiumAsync(RiskDTO riskDTO, int endorsementNumber, bool fromPolicy, int optionId = 0, bool forIssuance = false);
        Task LockRaterVersionAsync(Guid riskId);
        Task<bool> ShouldUpdateXpRaterAsync(Guid riskDetailId);
        Task<string> InitializeRaterVersionAsync(Guid riskId);
        Task<ExperienceRaterDTO> GetExperienceRaterDtoAsync(Guid riskId);
        Task<JObject> UploadExperienceRaterAsync(Guid riskDetailId, string filename, byte[] content);
        Task<RaterVersionDTO> GetLatestRaterVersionAsync();
        Task UpdateToLatestRaterVersionAsync(Guid riskId,Guid? riskDetailId = null);
    }
}
