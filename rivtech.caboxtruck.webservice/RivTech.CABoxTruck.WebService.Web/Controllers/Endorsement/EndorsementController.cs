using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using Quartz.Impl;
using RivTech.CABoxTruck.WebService.BackgroundServices.Jobs;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Exceptions;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.ProcessQueuing;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission;
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
    public class EndorsementController : ControllerBase
    {
        private readonly IEndorsementService _endorsementService;
        private readonly IEndorsementChangeItemModifierResolver _endorsementChangeItemModifierResolver;
        private readonly IEndorsementProcessStatusLogService _statusLogService;
        private readonly IEndorsementProcessQueueService _processQueueService;
        //private readonly IEndorsementProcessScheduler _endorsementProcessScheduler;
        private readonly IScheduler _scheduler;
        private readonly ILogger<EndorsementController> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IServiceProvider _serviceProvider;

        public EndorsementController(
            ILogger<EndorsementController> logger,
            IEndorsementService endorsementService,
            IEndorsementChangeItemModifierResolver endorsementChangeItemModifierResolver,
            IEndorsementProcessStatusLogService statusLogService,
            IEndorsementProcessQueueService processQueueService,
            //IEndorsementProcessScheduler endorsementProcessScheduler,
            IScheduler scheduler,
            IServiceScopeFactory serviceScopeFactory, IServiceProvider serviceProvider
        )
        {
            _logger = logger;
            _endorsementService = endorsementService;
            _endorsementChangeItemModifierResolver = endorsementChangeItemModifierResolver;
            _statusLogService = statusLogService;
            _processQueueService = processQueueService;
            //_endorsementProcessScheduler = endorsementProcessScheduler;
            _scheduler = scheduler;
            _serviceScopeFactory = serviceScopeFactory;
            _serviceProvider = serviceProvider;
        }

        [HttpGet("PremiumChanges/{id}")]
        [Produces(typeof(EndorsementPremiumChangeSummaryDTO))]
        public async Task<IActionResult> GetPremiumChanges([FromRoute] Guid id)
        {
            try
            {
                var resources = await _endorsementService.GetPremiumChanges(id);
                return Ok(resources);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("CalculatePremiumChanges/{id}")]
        public async Task<IActionResult> CalculatePremiumChanges([FromRoute] Guid id, [FromRoute] DateTime effectiveDate)
        {
            await _endorsementService.CalculatePremiumChanges(id, effectiveDate, 1);
            return Ok();
        }

        [HttpPut]
        [Route("UpdateEffectiveDate/{id}")]
        public async Task<IActionResult> UpdateEffectiveDate([FromRoute] Guid id, string effectiveDate)
        {
            try
            {
                var date = DateTime.Parse(effectiveDate);
                await _endorsementService.UpdateEffectiveDate(id, date);
                return Ok(date);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("UpdateEffectiveDateByButton/{id}")]
        public async Task<IActionResult> UpdateEffectiveDateByButton([FromRoute] Guid id, string effectiveDate)
        {
            try
            {
                var date = DateTime.Parse(effectiveDate);
                await _endorsementService.UpdateEffectiveDateByButton(id, date);
                return Ok(date);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("GeneratePolicyNumber/{id}")]
        public async Task<IActionResult> GeneratePolicyNumber([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _endorsementService.GeneratePolicyNumber(id));
            }
            catch (Exception e)
            {
                _logger.LogError("Error Generating Policy Number.", e);
                return new StatusCodeResult(500);
            }
        }

        [HttpPost("QueueProcess")]
        public async Task<IActionResult> QueueProcess([FromBody] EndorsementProcessQueue request)
        {
            var a = JsonConvert.SerializeObject(request);

            if (request.ProcessType != "IssueEndorsement")
            {
                var queue = await _processQueueService.QueueAsync(request);
                return Ok(queue);
            }
            else
            {
                var jobKey = JobKey.Create($"{nameof(EndorsementProcessJob)}-Job", $"{nameof(EndorsementProcessJob)}-Group");

                Dictionary<string, string> data = new Dictionary<string, string>();

                data = new Dictionary<string, string>()
                {
                    { "riskDetailId", request.RiskDetailId.ToString() },
                    { "effectiveDate", request.EffectiveDate.Value.ToString() },
                    { "endorsementText", request.EndorsementText },
                    { "policyChanges", request.PolicyChanges },
                };

                var jobData = new JobDataMap(data);

                await _scheduler.TriggerJob(jobKey, jobData);

                return Ok(request);
            }
        }

        [AllowAnonymous]
        [HttpPost("EndorsementProcess")]
        public async Task<IActionResult> EndorsementProcess()
        {
            var jobKey = JobKey.Create($"{nameof(EndorsementProcessJob)}-Job", $"{nameof(EndorsementProcessJob)}-Group");
            await _scheduler.TriggerJob(jobKey);
            return Ok();
        }

        [HttpDelete("DequeueProcess/{riskId}")]
        public async Task<IActionResult> DequeueProcess([FromRoute] Guid riskId)
        {
            await _processQueueService.DequeueByRiskAsync(riskId);
            return Ok();
        }

        [HttpGet("GetProcessQueue/{riskId}")]
        public async Task<IActionResult> GetProcessQueue([FromRoute] Guid riskId)
        {
            var queue = await _processQueueService.GetByRiskAsync(riskId);
            return Ok(queue);
        }

        //[HttpPost("ProcessEndorsement/{id}/{effectiveDate}")]
        //public async Task<IActionResult> ProcessEndorsement([FromRoute] Guid id, string effectiveDate)
        //{
        //    try
        //    {
        //        //await _endorsementService.GetEndorsementChanges(id);
        //        return Ok(await _endorsementService.ProcessEndorsement(id, DateTime.Parse(effectiveDate)));
        //    }
        //    catch (OutOfSequenceException e)
        //    {
        //        _logger.LogError(e, e.Message);
        //        return BadRequest(e.Message);
        //    }
        //    catch (OutOfSequenceValidationException e)
        //    {
        //        _logger.LogError(e, e.Message);
        //        return BadRequest(e.Message);
        //    }
        //    catch (RaterException e)
        //    {
        //        _logger.LogError(e, e.Message);
        //        return BadRequest(e.Message);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //        //_logger.LogError("Error processing endorsement.", e);
        //        //return new StatusCodeResult(500);
        //    }
        //}

        [HttpGet("GetChanges/{id}")]
        public async Task<IActionResult> GetChanges([FromRoute] Guid id)
        {
            //try
            //{
            await _endorsementService.GetEndorsementChanges(id);
            return Ok();
            //}
            //catch (Exception e)
            //{
            //    _logger.LogError("Error processing endorsement.", e);
            //    return new StatusCodeResult(500);
            //}
        }

        [HttpGet("StatusLogs/{riskId}")]
        public async Task<IActionResult> GetStatusLogs([FromRoute] Guid riskId)
        {
            return Ok(await _statusLogService.GetByRiskAsync(riskId));
        }

        [HttpDelete("ClearStatusLogs/{riskId}")]
        public async Task<IActionResult> ClearStatusLogs([FromRoute] Guid riskId)
        {
            await _statusLogService.DeleteAsync(riskId);
            return Ok();
        }


        [HttpGet("EffectiveDatePrompted/{riskId}")]
        [Produces(typeof(bool))]
        public async Task<IActionResult> EndorsementEffectiveDatePrompted([FromRoute] Guid riskId)
        {
            bool prompted = await _endorsementService.HasEffectiveDatePromptedAsync(riskId);
            return Ok(prompted);
        }

        [HttpPut("UpdateEffectiveDatePrompted/{riskId}")]
        [Produces(typeof(bool))]
        public async Task<IActionResult> UpdateEffectiveDatePrompted([FromRoute] Guid riskId, [FromBody] dynamic body)
        {
            await _endorsementService.UpdateEffectiveDatePromptedAsync(riskId, bool.Parse(body.prompted.ToString()));
            return Ok();
        }

        [HttpGet("resolver")]
        public async Task<IActionResult> TestResolver()
        {
            IEndorsementChangeItem changeItem = new BusinessNameChangeItem();
            var modifier = _endorsementChangeItemModifierResolver.Resolve(changeItem);
            return Ok();
        }

    }
}

