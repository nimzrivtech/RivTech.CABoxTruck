using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SASURLGeneratorController: ControllerBase
    {
        private readonly ISASURLGeneratorService _SASURLGeneratorService;
        public SASURLGeneratorController(ISASURLGeneratorService SASGeneratorService)
        {
            _SASURLGeneratorService = SASGeneratorService;
        }

        /// <summary>
        /// Generate a Shared Access Signature for the storage container URL / resource
        /// </summary>
        /// <param name="url">The URL of the Storage Container resource</param>
        /// <returns>An URL with the Shared Access Signature</returns>
        [HttpPost]
        public ActionResult<Uri> GenerateSASURL(string url)
        {
            try
            {
                return Ok(_SASURLGeneratorService.GenerateSharedAccessSignatureURL(url));
            }
            catch (Exception msg)
            {
                return StatusCode(500, msg);
            }
        }

        [HttpPost("GenerateSASURL2")]
        public ActionResult<Uri> GenerateSASURL2(string url, string newFilename)
        {
            try
            {
                return Ok(_SASURLGeneratorService.GenerateSharedAccessSignatureURL(url, newFilename));
            }
            catch (Exception msg)
            {
                return StatusCode(500, msg);
            }
        }
    }
}
