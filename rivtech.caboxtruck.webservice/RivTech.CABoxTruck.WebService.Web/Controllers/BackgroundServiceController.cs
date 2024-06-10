using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using Quartz.Impl;
using RivTech.CABoxTruck.WebService.BackgroundServices.Jobs;
using RivTech.CABoxTruck.WebService.Data.Common;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Exceptions;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.ProcessQueuing;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.DTO.Notification;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission;
using RivTech.CABoxTruck.WebService.Service.Services;
using RivTech.CABoxTruck.WebService.Service.Services.BackgroundServices.ISchedulers;
using RivTech.CABoxTruck.WebService.Service.Services.BackgroundServices.JobFactories;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.Validators;
using RivTech.CABoxTruck.WebService.Service.Services.Submission;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BackgroundServiceController : ControllerBase
    {
        private readonly IScheduler _scheduler;
        private readonly ILogger<EndorsementController> _logger;
        private readonly long _userId = -1;
        private readonly IRiskService _riskService;


        public BackgroundServiceController(
            ILogger<EndorsementController> logger,
            IScheduler scheduler,
            IRiskService riskService
        )
        {
            _logger = logger;
            _scheduler = scheduler;
            _userId = Services.GetCurrentUser();
            _riskService = riskService;
        }

        [HttpPost("TriggerBackgroundProcess")]
        public async Task<IActionResult> TriggerBackgroundProcess([FromBody] NotificationFilterDTO filter)
        {
            RiskDTO riskItem = await _riskService.GetByIdIncludeAsync(filter.NewRiskDetailId != null ? filter.NewRiskDetailId.Value : filter.OldRiskDetailId.Value);
            if (riskItem.Status != RiskDetailStatus.Active || (filter.OldRiskDetailId != null && filter.NewRiskDetailId != null))
            {
                // (riskItem.SubStatus != RiskDetailStatus.PendingEndorsement || 
                throw new Exception("Unable to issue endorsement. Please try again.");
            }
            filter.UserId = _userId;

            var notificationFilterDTO = JsonConvert.SerializeObject(filter);

            Dictionary<string, string> data = new Dictionary<string, string>();

            JobKey jobKey = null;

            if (filter.Category == "IssueEndorsement")
            {
                jobKey = JobKey.Create($"{nameof(EndorsementProcessJob)}-Job", $"{nameof(EndorsementProcessJob)}-Group");


                //data = new Dictionary<string, string>()
                //{
                //    { "riskDetailId", request.RiskDetailId.ToString() },
                //    { "effectiveDate", request.EffectiveDate.Value.ToString() },
                //    { "endorsementText", request.EndorsementText },
                //    { "policyChanges", request.PolicyChanges },
                //};

                data = new Dictionary<string, string>()
                {
                    { "notificationFilterDTO", notificationFilterDTO },
                };

            }
            else if (filter.Category == "Issuance")
            {
                jobKey = JobKey.Create($"{nameof(IssuanceProcessJob)}-Job", $"{nameof(IssuanceProcessJob)}-Group");

                data = new Dictionary<string, string>()
                {
                    { "notificationFilterDTO", notificationFilterDTO },
                };

            }
            else if (filter.Category == "IssuanceDefault")
            {
                jobKey = JobKey.Create($"{nameof(IssuanceDefaultProcessJob)}-Job", $"{nameof(IssuanceDefaultProcessJob)}-Group");

                data = new Dictionary<string, string>()
                {
                    { "notificationFilterDTO", notificationFilterDTO },
                };

            }
            else if (filter.Category == "Reset")
            {
                jobKey = JobKey.Create($"{nameof(ResetProcessJob)}-Job", $"{nameof(ResetProcessJob)}-Group");

                data = new Dictionary<string, string>()
                {
                    { "notificationFilterDTO", notificationFilterDTO },
                };

            }

            var jobData = new JobDataMap(data);

            await _scheduler.TriggerJob(jobKey, jobData);

            return Ok();
        }

    }
}

