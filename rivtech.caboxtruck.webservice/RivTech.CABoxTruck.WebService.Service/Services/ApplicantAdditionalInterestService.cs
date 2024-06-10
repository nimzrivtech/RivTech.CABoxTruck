using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories.Submission;
using RivTech.CABoxTruck.WebService.DomainEntities;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Submission.Applicant;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.DomainEntities.Security;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class ApplicantAdditionalInterestService : IApplicantAdditionalInterestService
    {
        private readonly IEntityAddressRepository _entityAddressRepository;
        private readonly IEntityRepository _entityRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IMapper _mapper;
        private readonly IRiskAdditionalInterestRepository _riskAdditionalInterestRepository;
        private readonly IQuoteOptionsService _quoteOptionsService;
        private readonly IRiskRepository _riskRepository;
        private readonly ITokenProvider _tokenProvider;
        private readonly IConfigurationSection _genericServiceSettings;
        private readonly string _genericBaseUrl;
        private readonly HttpClient _client;
        public IConfiguration Configuration { get; }
        private readonly IBackgroundServiceChecker _backgroundServiceChecker;

        public ApplicantAdditionalInterestService(
            IRiskAdditionalInterestRepository riskAdditionalInterestRepository,
            IEntityAddressRepository entityAddressRepository,
            IEntityRepository entityRepository,
            IAddressRepository addressRepository,
            IRiskDetailRepository riskDetailRepository,
            IQuoteOptionsService quoteOptionsService,
            IRiskRepository riskRepository,
            IMapper mapper,
            IConfiguration configuration,
            IBackgroundServiceChecker backgroundServiceChecker,
            ITokenProvider tokenProvider,
            IHttpClientFactory factory)
        {
            _riskAdditionalInterestRepository = riskAdditionalInterestRepository;
            _entityAddressRepository = entityAddressRepository;
            _entityRepository = entityRepository;
            _addressRepository = addressRepository;
            _riskDetailRepository = riskDetailRepository;
            _quoteOptionsService = quoteOptionsService;
            _mapper = mapper;
            _riskRepository = riskRepository;
            _backgroundServiceChecker = backgroundServiceChecker;
            Configuration = configuration;
            _client = factory.CreateClient("agencyClient");
            _genericServiceSettings = Configuration.GetSection("GenericService");
            //_genericBaseUrl = "https://app-generic-api-prodtest.azurewebsites.net/api";
            _genericBaseUrl = _genericServiceSettings.GetValue<string>("BaseUrl");
        }

        public async Task<IEnumerable<RiskAdditionalInterestDTO>> GetByRiskDetailIdAsync(Guid riskDetailId, bool fromEndorsement = false)
        {
            var additionalInterests = await _riskAdditionalInterestRepository
                .GetAsync(ai => ai.RiskDetailId == riskDetailId, ai => ai.Entity);
            var additionalInterestsDto = new List<RiskAdditionalInterestDTO>();

            var previousAdditionalInterest = new List<RiskAdditionalInterest>();
            if (fromEndorsement)
            {
                var currentRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
                var previousRiskDetails = await _riskDetailRepository.GetAsync(riskDetail => riskDetail.RiskId == currentRiskDetail.RiskId && riskDetail.Id != currentRiskDetail.Id);
                var previousRiskDetailId = previousRiskDetails.OrderByDescending(riskDetail => riskDetail.UpdatedDate).Select(riskDetail => riskDetail.Id).First();
                var previousRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(previousRiskDetailId);
                previousAdditionalInterest = _riskAdditionalInterestRepository.GetAsync(x => x.RiskDetailId == previousRiskDetail.Id).Result.ToList();
            }

            foreach (var ai in additionalInterests)
            {
                var entityAddress = await _entityAddressRepository
                    .FindAsync(ea => ea.EntityId == ai.EntityId, ea => ea.Address);

                var additionInterestDto = _mapper.Map<RiskAdditionalInterestDTO>(ai);
                additionInterestDto.Name = ai.Entity.FullName;
                additionInterestDto.Address = entityAddress?.Address.StreetAddress1;
                additionInterestDto.City = entityAddress?.Address.City;
                additionInterestDto.State = entityAddress?.Address.State;
                additionInterestDto.ZipCode = entityAddress?.Address.ZipCode;
                additionInterestDto.IsActive = ai.RemoveProcessDate == null ? true : false;

                if (fromEndorsement)
                {
                    var isNewAddtlInsured = previousAdditionalInterest.All(oldObj => oldObj.AddProcessDate != ai.AddProcessDate);
                    if (!isNewAddtlInsured)
                        additionInterestDto.fromPreviousEndorsement = true;
                }

                additionalInterestsDto.Add(additionInterestDto);
            }

            return additionalInterestsDto;
        }

        public async Task<IEnumerable<RiskAdditionalInterestDTO>> GetByRiskDetailIdActiveAsync(Guid riskId)
        {
            var additionalInterests = await _riskAdditionalInterestRepository
                .GetAsync(ai => ai.RiskDetailId == riskId && ai.RemoveProcessDate == null, ai => ai.Entity);
            var additionalInterestsDto = new List<RiskAdditionalInterestDTO>();
            foreach (var ai in additionalInterests)
            {
                var entityAddress = await _entityAddressRepository
                    .FindAsync(ea => ea.EntityId == ai.EntityId, ea => ea.Address);

                var additionInterestDto = _mapper.Map<RiskAdditionalInterestDTO>(ai);
                additionInterestDto.Name = ai.Entity.FullName;
                additionInterestDto.Address = entityAddress?.Address.StreetAddress1;
                additionInterestDto.City = entityAddress?.Address.City;
                additionInterestDto.State = entityAddress?.Address.State;
                additionInterestDto.ZipCode = entityAddress?.Address.ZipCode;
                additionalInterestsDto.Add(additionInterestDto);
            }

            return additionalInterestsDto;
        }

        public async Task<RiskAdditionalInterestDTO> GetAsync(Guid id)
        {
            var result = await _riskAdditionalInterestRepository
                .FindAsync(ai => ai.Id == id, ai => ai.Entity);
            return new RiskAdditionalInterestDTO
            {
                Id = result.Id,
                RiskDetailId = result.RiskDetailId,
                AdditionalInterestTypeId = result.AdditionalInterestTypeId
            };
        }
        public async Task<(List<string> cities, string ZipCode, string Status)> GetCityState(string zipCode)
        {
            var response = await _client.GetAsync(_genericBaseUrl + "/ZipCodes/" + zipCode);
            string stateCode = "";
            List<string> cities = new List<string>();
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                JArray jsonArray = JArray.Parse(jsonResponse);
                JObject firstObject = (JObject)jsonArray[0];
                stateCode = (string)firstObject["stateCode"];
                foreach (JObject cityList in jsonArray)
                {
                    var f = (string)firstObject["city"];
                    cities.Add((string)cityList["city"]);
                }

            }
            return (cities, stateCode, response.IsSuccessStatusCode ? "success" : "failed");
        }
        private async Task AddAuthHeaderToken()
        {
            _client.DefaultRequestHeaders.Authorization = null;
            var token = Data.Common.Services.GetAuthToken();
            if (_backgroundServiceChecker.IsFromBackgroundService())
            {
                token = await _tokenProvider.GetAdminTokenAsync();
            }

            if (!String.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<bool> validateAddress(RiskAdditionalInterestDTO model)
        {
            await AddAuthHeaderToken();
            // Call asynchronous network methods in a try/catch block to handle exceptions
            (List<string> cities, string stateCode, string status) = await GetCityState(model.ZipCode);
            bool cityMatch = cities.Exists(it => it == model.City) || (cities.Count == 1 && model.City == null);
            if (model.State != stateCode || !cityMatch || status == "failed")
            {
                throw new Exception("Invalid Address");
            }
            return true;
        }

        public async Task<Guid> InsertAsync(RiskAdditionalInterestDTO model)
        {
            await validateAddress(model);
            var additionalInterest = _mapper.Map<RiskAdditionalInterest>(model);
            additionalInterest.AddProcessDate = DateTime.Now;
            additionalInterest.AddedBy = 0;
            additionalInterest.Entity = new Entity {FullName = model.Name};
            var result = await _riskAdditionalInterestRepository.AddAsync(additionalInterest);

            await _entityAddressRepository.AddAsync(new EntityAddress
            {
                Id = Guid.NewGuid(),
                EntityId = result.EntityId,
                AddressTypeId = model.AdditionalInterestTypeId,
                Address = new Address
                {
                    StreetAddress1 = model.Address,
                    City = model.City,
                    State = model.State,
                    ZipCode = model.ZipCode
                }
            });

            await _riskAdditionalInterestRepository.SaveChangesAsync();
            await _quoteOptionsService.CalculateAdditionalInterests(additionalInterest.RiskDetailId); // update premiums
            return result.Id;
        }

        public async Task<bool> RemoveAsync(Guid id, bool fromEndorsement, DateTime expirationDate)
        {
            var data = await _riskAdditionalInterestRepository.FindAsync(x => x.Id == id);

            if (!fromEndorsement)
            {
                var entityAddress = await _entityAddressRepository.FindAsync(ea => ea.EntityId == data.EntityId,
                ea => ea.Entity, ea => ea.Address);
                _riskAdditionalInterestRepository.Remove(data);
                _entityAddressRepository.Remove(entityAddress);
                _entityRepository.Remove(entityAddress.Entity);
                _addressRepository.Remove(entityAddress.Address);
                await _riskAdditionalInterestRepository.SaveChangesAsync();
                await _quoteOptionsService.CalculateAdditionalInterests(data.RiskDetailId); // update premiums
                return true;
            }

            var currentRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(data.RiskDetailId);
            var previousRiskDetails = await _riskDetailRepository.GetAsync(riskDetail => riskDetail.RiskId == currentRiskDetail.RiskId && riskDetail.Id != currentRiskDetail.Id);
            var previousRiskDetailId = previousRiskDetails.OrderByDescending(riskDetail => riskDetail.UpdatedDate).Select(riskDetail => riskDetail.Id).First();
            var previousRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(previousRiskDetailId);
            var previousAdditionalInterest = await _riskAdditionalInterestRepository.GetAsync(x => x.RiskDetailId == previousRiskDetail.Id);

            var isNewAddtlInsured = previousAdditionalInterest.All(oldObj => oldObj.AddProcessDate != data.AddProcessDate);
            if (isNewAddtlInsured)
            {
                // Hard Delete if Endorsement new added interest
                var entityAddress = await _entityAddressRepository.FindAsync(ea => ea.EntityId == data.EntityId,
                ea => ea.Entity, ea => ea.Address);
                _riskAdditionalInterestRepository.Remove(data);
                _entityAddressRepository.Remove(entityAddress);
                _entityRepository.Remove(entityAddress.Entity);
                _addressRepository.Remove(entityAddress.Address);
                await _riskAdditionalInterestRepository.SaveChangesAsync();
                await _quoteOptionsService.CalculateAdditionalInterests(data.RiskDetailId); // update premiums
                return true;
            }

            data.ExpirationDate = expirationDate;
            data.RemoveProcessDate = DateTime.Now;
            _riskAdditionalInterestRepository.Update(data);
            await _riskAdditionalInterestRepository.SaveChangesAsync();
            await _quoteOptionsService.CalculateAdditionalInterests(data.RiskDetailId); // update premiums
            return false;
        }

            public async Task<Guid> UpdateAsync(RiskAdditionalInterestDTO model)
        {
            await validateAddress(model);
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(model.RiskDetailId);
            var risk = await _riskRepository.FindAsync(riskDetail.RiskId);

            var additionalInterest = await _riskAdditionalInterestRepository.FindByIdIncludeAsync((Guid)model.Id);

            if (additionalInterest.IsAutoLiability != model.IsAutoLiability ||
            additionalInterest.IsGeneralLiability != model.IsGeneralLiability ||
            additionalInterest.IsPrimaryNonContributory != model.IsPrimaryNonContributory ||
            additionalInterest.IsWaiverOfSubrogation != model.IsWaiverOfSubrogation
            )
            {
                throw new Exception("Invalid updating in one of the toggles. Please try again");
            }
            model.AddProcessDate = additionalInterest.AddProcessDate;
            _mapper.Map(model, additionalInterest);
            additionalInterest.Entity.FullName = model.Name;

            //Update 30 day notice on current risk detail (for policy)
            if (risk?.CurrentRiskDetailId != null && risk?.CurrentRiskDetailId != model.RiskDetailId)
            {
            var currentAdditionalInterests = (await _riskAdditionalInterestRepository.GetAllIncludeAsync((Guid)risk.CurrentRiskDetailId)).ToList();
            if(currentAdditionalInterests?.Any() ?? false)
            {
                var currentAdditionalInterest = currentAdditionalInterests.FirstOrDefault(r => r.AdditionalInterestTypeId == additionalInterest.AdditionalInterestTypeId
                    && r.Entity.FullName == additionalInterest.Entity.FullName
                    && r.Entity.EntityAddresses.FirstOrDefault()?.Address?.City == additionalInterest.Entity.EntityAddresses.FirstOrDefault()?.Address?.City
                    && r.Entity.EntityAddresses.FirstOrDefault()?.Address?.State == additionalInterest.Entity.EntityAddresses.FirstOrDefault()?.Address?.State
                    && r.Entity.EntityAddresses.FirstOrDefault()?.Address?.ZipCode == additionalInterest.Entity.EntityAddresses.FirstOrDefault()?.Address?.ZipCode);

                    if(currentAdditionalInterest != null)
                        currentAdditionalInterest.Is30DayNotice = model.Is30DayNotice;
                }
            }

                var entityAddress = await _entityAddressRepository.FindAsync(ea => ea.EntityId == additionalInterest.EntityId, ea => ea.Address);
            entityAddress.Address.StreetAddress1 = model.Address;
            entityAddress.Address.City = model.City;
            entityAddress.Address.State = model.State;
            entityAddress.Address.ZipCode = model.ZipCode;

            await _riskAdditionalInterestRepository.SaveChangesAsync();
            await _quoteOptionsService.CalculateAdditionalInterests(additionalInterest.RiskDetailId); // update premiums
            return additionalInterest.Id;
        }

        public async Task<bool> UpdateBlanketCoverage(BlanketCoverageDTO model)
        {
            var risk = await _riskDetailRepository.FindAsync(r => r.Id == model.RiskDetailId);
            _mapper.Map(model, risk);
            await _riskDetailRepository.SaveChangesAsync();

            var interests  = await _riskAdditionalInterestRepository.GetAsync(ai => ai.RiskDetailId == model.RiskDetailId);

            foreach (var interest in interests)
            {
                if (model.IsWaiverOfSubrogation.GetValueOrDefault())
                {
                    interest.IsWaiverOfSubrogation = true;
                    _riskAdditionalInterestRepository.Entry(interest).Property(p => p.IsWaiverOfSubrogation).IsModified = true;
                }
                if (model.IsPrimaryNonContributory.GetValueOrDefault())
                {
                    interest.IsPrimaryNonContributory = true;
                    _riskAdditionalInterestRepository.Entry(interest).Property(p => p.IsPrimaryNonContributory).IsModified = true;
                }
            }

            await _riskAdditionalInterestRepository.SaveChangesAsync();
            await _quoteOptionsService.CalculateAdditionalInterests(model.RiskDetailId); // update premiums

            return true;
        }

        public async Task<AdditionalInterestPagingDTO> GetByRiskDetailIdAsync(AdditionalInterestFilterDTO AdditionalInterestFilter)
        {
            var previousAdditionalInterest = new List<RiskAdditionalInterest>();
            if (AdditionalInterestFilter.fromEndorsement)
            {
                var currentRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(AdditionalInterestFilter.RiskDetailId.Value);
                var previousRiskDetails = await _riskDetailRepository.GetAsync(riskDetail => riskDetail.RiskId == currentRiskDetail.RiskId && riskDetail.Id != currentRiskDetail.Id);
                var previousRiskDetailId = previousRiskDetails.OrderByDescending(riskDetail => riskDetail.UpdatedDate).Select(riskDetail => riskDetail.Id).First();
                var previousRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(previousRiskDetailId);
                previousAdditionalInterest = _riskAdditionalInterestRepository.GetAsync(x => x.RiskDetailId == previousRiskDetail.Id).Result.ToList();
            }


            Expression<Func<RiskAdditionalInterest, bool>> expression = x => x.RiskDetailId == AdditionalInterestFilter.RiskDetailId;

            var additionalInterestsDto = new List<RiskAdditionalInterestDTO>();

            

            var (result, paginationDetails) = await _riskAdditionalInterestRepository.GetWithPagingAsync(expression, AdditionalInterestFilter.PageNumber, AdditionalInterestFilter.PageSize, AdditionalInterestFilter.SortField, AdditionalInterestFilter.Order);

            foreach (var item in result.ToList())
            {
                var entityAddress = await _entityAddressRepository
                        .FindAsync(ea => ea.EntityId == item.EntityId, ea => ea.Address);

                var additionInterestDto = _mapper.Map<RiskAdditionalInterestDTO>(item);
                additionInterestDto.Name = item.Entity.FullName;
                additionInterestDto.Address = entityAddress?.Address.StreetAddress1;
                additionInterestDto.City = entityAddress?.Address.City;
                additionInterestDto.State = entityAddress?.Address.State;
                additionInterestDto.ZipCode = entityAddress?.Address.ZipCode;
                additionInterestDto.IsActive = item.RemoveProcessDate == null ? true : false;

                if (AdditionalInterestFilter.fromEndorsement)
                {
                    var isNewAddtlInsured = previousAdditionalInterest.All(oldObj => oldObj.AddProcessDate != item.AddProcessDate);
                    if (!isNewAddtlInsured)
                        additionInterestDto.fromPreviousEndorsement = true;
                }

                additionalInterestsDto.Add(additionInterestDto);
            }

            return new AdditionalInterestPagingDTO()
            {
                PaginationDetails = paginationDetails,
                AdditionalInterest = additionalInterestsDto

            };

        }
    }
}