using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using Quartz;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DomainEntities;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Notification;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Report;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission;
using RivTech.CABoxTruck.WebService.Service.Services;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement;
using RivTech.CABoxTruck.WebService.Service.Services.Report;
using System;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.BackgroundServices.Jobs
{
    public class IssuanceDefaultProcessJob : IJob
    {
        private readonly ILogger<IssuanceDefaultProcessJob> _logger;
        IServiceScopeFactory _serviceScopeFactory;

        public IssuanceDefaultProcessJob(
            ILogger<IssuanceDefaultProcessJob> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var scope = _serviceScopeFactory.CreateScope();
            var sp = scope.ServiceProvider;
            var endorsementService = sp.GetRequiredService<IEndorsementService>();
            var backgroundServiceChecker = sp.GetRequiredService<IBackgroundServiceChecker>();
            var notificationService = sp.GetRequiredService<INotificationService>();
            var errorLogService = sp.GetRequiredService<ILog4NetService>();

            var dataMap = context.MergedJobDataMap;

            var notificationFilterDTO = JsonConvert.DeserializeObject<NotificationFilterDTO>(dataMap.GetString("notificationFilterDTO"));

            try
            {
                notificationFilterDTO.Title = "Premium calculation in progress";
                notificationFilterDTO.NotificationStatus = "InProgress";
                notificationFilterDTO.CompletionPercentage = 1;
                notificationFilterDTO.IsRead = false;
                notificationFilterDTO.DateUpdated = DateTime.Now;

                var notificationId = await notificationService.CreateNotifications(notificationFilterDTO);

                notificationFilterDTO.NotificationId = notificationId;

                ((BackgroundServiceChecker)backgroundServiceChecker).TagAsBackgroundService();
                await endorsementService.ProcessEndorsementDefault(notificationFilterDTO.RiskId.Value, notificationFilterDTO);
            }
            catch (Exception e)
            {
                _logger.LogError("Error occured while processing Endorsement Process Background Service. {0}", e);
                
                notificationFilterDTO.Title = "Premium calculation failed";
                notificationFilterDTO.NotificationStatus = "Failed";
                notificationFilterDTO.DateUpdated = DateTime.Now;

                await notificationService.UpdateNotificationStatus(notificationFilterDTO);
                ErrorLogDTO errLog = new ErrorLogDTO
                {
                    UserId = notificationFilterDTO.UserId,
                    ErrorMessage = e.ToString(),
                    JsonMessage = JsonConvert.SerializeObject(notificationFilterDTO),
                    CreatedDate = DateTime.Now
                };
                await errorLogService.ErrorAsync(errLog);
            }
        }
    }
}
