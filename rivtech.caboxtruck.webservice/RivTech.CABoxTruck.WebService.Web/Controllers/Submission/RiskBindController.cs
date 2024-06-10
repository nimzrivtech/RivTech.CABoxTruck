using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission;
using RivTech.CABoxTruck.WebService.Service.Services.Submission;

namespace RivTech.CABoxTruck.WebService.Controllers.Submission
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class RiskBindController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IRiskService _riskService;
        private readonly ISubmissionService _submissionService;

        public RiskBindController(
            ITransactionService transactionService,
            IRiskService riskService,
            ISubmissionService submissionService
        )
        {
            _submissionService = submissionService;
            _transactionService = transactionService;
            _riskService = riskService;
        }

        [HttpGet("Issue/{id}")]
        //[Produces(typeof(List<RiskDTO>))]
        public async Task<IActionResult> Issue([FromRoute] Guid id)
        {
            try
            {
                var res = await _transactionService.IssueSubmission(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("Reset/{id}")]
        //[Produces(typeof(List<RiskDTO>))]
        public async Task<IActionResult> Reset([FromRoute] Guid id)
        {
            try
            {
                var res = await _transactionService.ResetPolicyChanges(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetPolicyChanges/{id}")]
        //[Produces(typeof(List<RiskDTO>))]
        public async Task<IActionResult> GetPolicyChanges([FromRoute] Guid id)
        {
            try
            {
                var res = await _transactionService.GetPolicyChanges(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("IssueEndorsement")]
        public async Task<IActionResult> IssueEndorsement([FromBody] JObject endorsementDetails)
        {
            try
            {
                Guid riskDetailId = Guid.Parse(endorsementDetails.SelectToken("riskDetailId").Value<string>());
                DateTime effectiveDate = endorsementDetails.SelectToken("effectiveDate").Value<DateTime>();
                string endorsementText = endorsementDetails.SelectToken("endorsementText").Value<string>();
                string policyChanges = endorsementDetails.SelectToken("policyChanges").Value<string>();

                var res = await _transactionService.IssueEndorsement(riskDetailId, effectiveDate, endorsementText, policyChanges);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("ManualCancellationNotice")]
        public async Task<IActionResult> ManualCancellationNotice([FromBody] JObject cancellationNoticeDetails)
        {
            try
            {
                Guid riskId = Guid.Parse(cancellationNoticeDetails.SelectToken("riskId").Value<string>());
                Guid riskDetailId = Guid.Parse(cancellationNoticeDetails.SelectToken("riskDetailId").Value<string>());
                DateTime noticeDate = cancellationNoticeDetails.SelectToken("noticeDate").Value<DateTime>();
                DateTime effectiveDate = cancellationNoticeDetails.SelectToken("effectiveDate").Value<DateTime>();
                string cancellationReasonType = cancellationNoticeDetails.SelectToken("cancellationReasonType").Value<string>();

                await _transactionService.ManualCancellationNotice(riskId, riskDetailId, noticeDate, effectiveDate, cancellationReasonType);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("CancelPolicy")]
        public async Task<IActionResult> CancelPolicy([FromBody] JObject cancellationDetails)
        {
            try
            {
                Guid riskDetailId = Guid.Parse(cancellationDetails.SelectToken("riskDetailId").Value<string>());
                DateTime effectiveDate = cancellationDetails.SelectToken("effectiveDate").Value<DateTime>();
                string cancellationReasonType = cancellationDetails.SelectToken("cancellationReasonType").Value<string>();
                bool isShortRate = cancellationDetails.SelectToken("isShortRate").Value<bool>();

                await _transactionService.CancelPolicy(riskDetailId, effectiveDate, cancellationReasonType, isShortRate);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return Ok(new { Message = ex.Message });
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("ReinstatePolicy/{id}")]
        public async Task<IActionResult> ReinstatePolicy([FromRoute] Guid id)
        {
            try
            {
                var res = await _transactionService.ReinstatePolicy(id);
                return Ok(res);
            }
            catch (ArgumentException ex)
            {
                return Ok(new { Message = ex.Message });
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("RescindPolicy/{id}")]
        public async Task<IActionResult> RescindPolicy([FromRoute] Guid id)
        {
            try
            {
                var res = await _transactionService.ManualRescindPolicy(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("RewritePolicy/{id}")]
        public async Task<IActionResult> RewritePolicy([FromRoute] Guid id)
        {
            try
            {
                await _transactionService.CancelPolicyForRewrite(id, DateTime.Now, true);
                var res = await _transactionService.RewritePolicyToSubmission(id, DateTime.Now);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("CopySubmission/{id}")]
        public async Task<IActionResult> CopySubmission([FromRoute] Guid id)
        {
            try
            {
                var res = await _transactionService.CopySubmission(id, DateTime.Now);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("RenewPolicy/{policyNumber}")]
        public async Task<IActionResult> RenewPolicy([FromRoute] string policyNumber)
        {
            try
            {
                var res = await _transactionService.RenewPolicy(policyNumber);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("DeleteSubmission/{submissionNumber}")]
        public async Task DeleteSubmission([FromRoute] string submissionNumber)
        {
            await _submissionService.DeleteSubmissionAsync(submissionNumber);
        }

        [HttpGet("GetPoliciesForRenewal/{date}")]
        public async Task<string[]> GetPoliciesForRenewal([FromRoute] DateTime date)
        {
            return await _riskService.GetPoliciesForRenewalAsync(date);
        }

        [HttpGet("GetExpiredPolicies/{date}")]
        public async Task<string[]> GetExpiredPolicies([FromRoute] DateTime date)
        {
            return await _riskService.GetExpiredPoliciesAsync(date);
        }

        [HttpPost("ExpirePolicy/{policyNumber}")]
        public async Task<IActionResult> ExpirePolicy([FromRoute] string policyNumber)
        {
            try
            {
                var res = await _transactionService.ExpirePolicy(policyNumber);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("AutoPolicyCancellation")]
        public async Task<IActionResult> AutoPolicyCancellation()
        {
            var jobDate = GetRequestDate(Request);
            await _transactionService.AutoPolicyCancellation(jobDate);
            return Ok();
        }

        [HttpPost]
        [Route("AutoNoticeCancellation")]
        public async Task<IActionResult> AutoNoticeCancellation()
        {
            var jobDate = GetRequestDate(Request);
            await _transactionService.AutoNoticeOfCancellation(jobDate);
            return Ok();
        }

        [HttpPost]
        [Route("AutoRescindNoticeCancellation")]
        public async Task<IActionResult> AutoRescindNoticeCancellation()
        {
            var jobDate = GetRequestDate(Request);
            await _transactionService.AutoRescindNoticeOfCancellation(jobDate);
            return Ok();
        }

        [HttpPost]
        [Route("AutoReinstateCancelledPolicies")]
        public async Task<IActionResult> AutoReinstateCancelledPolicies()
        {
            var jobDate = GetRequestDate(Request);
            await _transactionService.AutoReinstateCancelledPolicy(jobDate);
            return Ok();
        }

        private DateTime GetRequestDate(HttpRequest request)
        {
            var jobDate = DateTime.Now;
            if (request.Headers.TryGetValue("userdate", out var headerValues))
            {
                jobDate = DateTime.Parse(headerValues.FirstOrDefault());
            }

            return jobDate;
        }

        [HttpPost]
        [Route("ManualRescindPNOCByRiskIdDataFix/{riskId}")]
        public async Task<IActionResult> ManualRescindPNOCByRiskIdDataFix([FromRoute] Guid riskId)
        {
            await _transactionService.ManualRescindPNOCByRiskIdDataFix(riskId);
            return Ok();
        }
    }
}
