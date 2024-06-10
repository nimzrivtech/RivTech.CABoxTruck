using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DomainEntities.Security;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Security
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IConfiguration _config;

        public TokenProvider(
            IConfiguration configuration
        ) {
            _config = configuration;
        }

        public async Task<string> GetAdminTokenAsync()
        {
            var client = new HttpClient();
            var apiUrl = _config["IdentityApi:Url"];
            var credential = new
            {
                username = _config["IdentityApi:Username"],
                password = _config["IdentityApi:Password"],
                appCode = 2
            };

            var content = new StringContent(JsonConvert.SerializeObject(credential), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{apiUrl}/auth", content);
            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase);
            var stringResponseContent = await response.Content.ReadAsStringAsync();
            dynamic responseContent = JsonConvert.DeserializeObject(stringResponseContent);

            return responseContent.token.ToString();
        }
    }
}
