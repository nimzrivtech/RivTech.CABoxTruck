using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO.Enum;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class ApplogService : IApplogService
    {
        private readonly IAppLogRepository _appLogRepository;
        public ApplogService(IAppLogRepository appLogRepository)
        {
            _appLogRepository = appLogRepository;
        }

        public async Task Save(string message, string jsonMessage)
        {
            await _appLogRepository.AddAsync(new Data.Entity.AppLog()
            {
                UserId = -1,
                Message = message,
                RequestValues = jsonMessage,
                CreatedDate = DateTime.Now
            });

            await _appLogRepository.SaveChangesAsync();
        }

        public async Task LogDocumentGeneration(string message, string jsonMessage, string response)
        {
            await _appLogRepository.AddAsync(new Data.Entity.AppLog()
            {
                UserId = Data.Common.Services.GetCurrentUser(),
                Message = message,
                RequestValues = jsonMessage,
                Type = AppLogType.DocGen.ToString(),
                CreatedDate = DateTime.Now,
                ResponseValues = response
            });

            await _appLogRepository.SaveChangesAsync();
        }
        public async Task LogRequestResponse(string message, string request, string response, string type)
        {
            await _appLogRepository.AddAsync(new Data.Entity.AppLog()
            {
                UserId = Data.Common.Services.GetCurrentUser(),
                Message = message,
                RequestValues = request,
                ResponseValues = response,
                CreatedDate = DateTime.Now,
                Type = type
            });

            await _appLogRepository.SaveChangesAsync();
        }
    }
}
