using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Security;
using RivTech.CABoxTruck.WebService.DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO.Generics;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class GenericsRepository : IGenericsRepository
    {
        private readonly HttpClient _client;
        private readonly IBackgroundServiceChecker _backgroundServiceChecker;
        private readonly ITokenProvider _tokenProvider;

        public GenericsRepository(
            IHttpClientFactory factory,
            IBackgroundServiceChecker backgroundServiceChecker,
            ITokenProvider tokenProvider
        )
        {
            _backgroundServiceChecker = backgroundServiceChecker;
            _tokenProvider = tokenProvider;
            _client = factory.CreateClient("genericClient");
        }

        public async Task<IEnumerable<HolidayScheduleDTO>> GetHolidaySchedule()
        {
            try
            {
                await AddAuthHeaderToken();
                var baseUrl = Environment.GetEnvironmentVariable("GenericService__BaseUrl");
                var response = await _client.GetAsync($"{baseUrl}/HolidaySchedule");

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("GetAgent API error");
                }

                return JsonConvert.DeserializeObject<List<HolidayScheduleDTO>>(jsonResponse);
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
            if (_backgroundServiceChecker.IsFromBackgroundService())
            {
                token = await _tokenProvider.GetAdminTokenAsync();
            }

            if (!string.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}


