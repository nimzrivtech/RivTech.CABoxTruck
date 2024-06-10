using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DomainEntities;
using RivTech.CABoxTruck.WebService.DomainEntities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class AgencyRepository : BaseRepository<Agency, Guid>, IAgencyRepository
    {
        private readonly HttpClient _client;
        public IConfiguration Configuration { get; }
        private readonly IConfigurationSection _agencyServiceSettings;
        private readonly string _agencyBaseUrl;
        private readonly IBackgroundServiceChecker _backgroundServiceChecker;
        private readonly ITokenProvider _tokenProvider;

        public AgencyRepository(AppDbContext context,
            IConfiguration configuration,
            IHttpClientFactory factory,
            IBackgroundServiceChecker backgroundServiceChecker,
            ITokenProvider tokenProvider
        ) : base(context)
        {
            Configuration = configuration;
            _backgroundServiceChecker = backgroundServiceChecker;
            _tokenProvider = tokenProvider;
            _client = factory.CreateClient("agencyClient");
            _agencyServiceSettings = Configuration.GetSection("AgencyService");
            _agencyBaseUrl = _agencyServiceSettings.GetValue<string>("BaseUrl");
        }

        public async Task<IEnumerable<Agency>> GetAllIncludeAsync()
        {
            return await _context.Set<Agency>()
                                .Include(config=> config.Entity)
                                .ToListAsync();
        }

        public async Task<IEnumerable<Agency>> GetAllIncludeAsync(Expression<Func<Agency, bool>> expression)
        {
            return await _context.Set<Agency>()
                                .Include(config => config.Entity)
                                .Where(expression)
                                .ToListAsync();
        }

        public async Task<List<Agency>> GetAllAgenciesByIds(List<Guid> ids)
        {
            try
            {
                string stringContent = JsonConvert.SerializeObject(ids);
                using HttpContent content = new StringContent(stringContent, Encoding.UTF8, "application/json");
                await AddAuthHeaderToken();
                var response = await _client.PostAsync($"{_agencyBaseUrl}/Agencies/getAllAgencyByIds", content);

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"GetAllAgenciesByIds API error: {response.ReasonPhrase}");
                }

                return JsonConvert.DeserializeObject<List<Agency>>(jsonResponse);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Agency> GetAgency(Guid agencyId)
        {
            try
            {
                await AddAuthHeaderToken();
                var response = await _client.GetAsync($"{_agencyBaseUrl}/Agencies/getagencydetails/{agencyId}");

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"GetAgency API error: {response.ReasonPhrase}");
                }

                return JsonConvert.DeserializeObject<Agency>(jsonResponse);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task AddAuthHeaderToken()
        {
            _client.DefaultRequestHeaders.Authorization = null;
            var token = Data.Common.Services.GetAuthToken();
            if(_backgroundServiceChecker.IsFromBackgroundService())
            {
                token = await _tokenProvider.GetAdminTokenAsync();
            }

            if (!string.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public class SubAgencyRepository : BaseRepository<SubAgency, Guid>, ISubAgencyRepository
    {
        public SubAgencyRepository(AppDbContext context)
           : base(context)
        {
        }

        public async Task<IEnumerable<SubAgency>> GetAllIncludeAsync()
        {
            return await _context.Set<SubAgency>()
                                .Include(config => config.Entity)
                                .ToListAsync();
        }

        public async Task<IEnumerable<SubAgency>> GetAllIncludeAsync(Expression<Func<SubAgency, bool>> expression)
        {
            return await _context.Set<SubAgency>()
                                .Include(config => config.Entity)
                                .Where(expression)
                                .ToListAsync();
        }
    }


}


