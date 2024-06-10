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
    public class ResetProcessJob : IJob
    {
        private readonly ILogger<ResetProcessJob> _logger;
        IServiceScopeFactory _serviceScopeFactory;
        //ITransactionService _transactionService;
        //IBackgroundServiceChecker _backgroundServiceChecker;

        public ResetProcessJob(
            ILogger<ResetProcessJob> logger, IServiceScopeFactory serviceScopeFactory)
        //ITransactionService transactionService,
        //IBackgroundServiceChecker backgroundServiceChecker)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            //_transactionService = transactionService;
            //_backgroundServiceChecker = backgroundServiceChecker;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var scope = _serviceScopeFactory.CreateScope();
            var sp = scope.ServiceProvider;
            var transactionService = sp.GetRequiredService<ITransactionService>();
            var backgroundServiceChecker = sp.GetRequiredService<IBackgroundServiceChecker>();
            var notificationService = sp.GetRequiredService<INotificationService>();
            var errorLogService = sp.GetRequiredService<ILog4NetService>();

            var dataMap = context.MergedJobDataMap;

            var notificationFilterDTO = JsonConvert.DeserializeObject<NotificationFilterDTO>(dataMap.GetString("notificationFilterDTO"));

            try
            {
                notificationFilterDTO.Title = "Policy reset in progress";
                notificationFilterDTO.NotificationStatus = "InProgress";
                notificationFilterDTO.CompletionPercentage = 1;
                notificationFilterDTO.IsRead = false;
                notificationFilterDTO.DateUpdated = DateTime.Now;

                var notificationId = await notificationService.CreateNotifications(notificationFilterDTO);

                notificationFilterDTO.NotificationId = notificationId;

                dynamic notificationDetails = JsonConvert.DeserializeObject<ExpandoObject>(notificationFilterDTO.NotificationDetails);

                Guid riskDetailId = Guid.Parse(notificationDetails.riskDetailId);
                DateTime effectiveDate = DateTime.Parse(notificationDetails.effectiveDate);
                string endorsementText = notificationDetails.endorsementText;
                string policyChanges = notificationDetails.policyChanges;


                ((BackgroundServiceChecker)backgroundServiceChecker).TagAsBackgroundService();
                await transactionService.ResetPolicyChanges(notificationFilterDTO.OldRiskDetailId.Value, notificationFilterDTO);
            }
            catch (Exception e)
            {
                _logger.LogError("Error occured while processing Endorsement Process Background Service. {0}", e);
                
                notificationFilterDTO.Title = "Policy reset failed";
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
