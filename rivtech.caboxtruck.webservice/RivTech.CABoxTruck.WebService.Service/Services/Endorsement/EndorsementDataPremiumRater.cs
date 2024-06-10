using AutoMapper;
using Newtonsoft.Json;
using NPOI.OpenXmlFormats.Dml.Chart;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement
{
    public class EndorsementDataPremiumRater : IEndorsementDataPremiumRater
    {
        private readonly IRaterService _raterService;
        private readonly IRiskService _riskService;
        private readonly IEndorsementProcessStatusLogService _statusLogService;

        public EndorsementDataPremiumRater(
            IRaterService raterService,
            IRiskService riskService,
            IEndorsementProcessStatusLogService statusLogService
        ) {
            _raterService = raterService;
            _riskService = riskService;
            _statusLogService = statusLogService;
        }

        public async Task PreloadAsync(IEndorsementData endorsementData)
        {
            var data = (EndorsementData)endorsementData;

            //Guid _riskDetailId = Guid.Parse(riskDetailId);
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
            var risk = await _riskService.GetByIdIncludeAsync(data.RiskDetailId);
            var brokerinfo = risk.BrokerInfo;
            short yearsInBusiness = risk.BusinessDetails?.YearsInBusiness ?? 0;

            if (brokerinfo?.EffectiveDate == null)
            {
                throw new RaterException("Inception Date is Required.");
            }

            DateTime inceptionDate = brokerinfo.EffectiveDate.Value;

            short optionNum = 1;
            if (risk.Binding != null)
            {
                optionNum = risk.Binding.BindOptionId ?? 1;
            }
            var covPrem = risk.RiskCoverages.FirstOrDefault(c => c.OptionNumber == optionNum);

            if (covPrem?.LiabilityExperienceRatingFactor == null && covPrem?.LiabilityScheduleRatingFactor == null)
            {
                throw new RaterException("Liability Schedule RF and Liability Experience RF is not defined");
            }

            _ = _raterService.PreloadPremiumRater(
                data.SubmissionNumber,
                risk.RaterVersionId,
                fromPolicy: true
            );
        }

        public async Task RateAsync(IEndorsementData endorsementData,  int endorsementNumber, bool forIssuance = false)
        {
            var data = (EndorsementData)endorsementData;


            if(!data.RiskDTO.Vehicles.Any(x => x.DeletedDate is null))
            {
                throw new RaterException($"No vehicles available to rate for Endorsement {data.EndorsementNumber}.");
            }

            await _statusLogService.LogAsync(data.RiskId, $"Preloading premium rater.");
            await _raterService.PreloadPremiumRater(
                data.SubmissionNumber,
                data.RaterVersionId,
                true
            );
            await _statusLogService.LogAsync(data.RiskId, $"Premium rater preloaded.");

            await ValidateData(data);


            await _statusLogService.LogAsync(data.RiskId, $"Rating annual premiums of Endorsement {data.EndorsementNumber}");
            await _raterService.CalculatePremiumAsync(
                data.RiskDTO,
                endorsementNumber,
                fromPolicy: true,
                forIssuance: forIssuance
            );
            await _statusLogService.LogAsync(data.RiskId, $"Done rating annual premiums of Endorsement {data.EndorsementNumber}");
        }

        private async Task ValidateData(EndorsementData data)
        {
            var errors = new List<string>();

            await _statusLogService.LogAsync(data.RiskId, $"Validating if there are existing vehicles.");
            if (!data.RiskDTO.Vehicles.Any(x => x.DeletedDate is null))
            {
                errors.Add($"No vehicle available to rate for Endorsement {data.EndorsementNumber}.");
            }

            await _statusLogService.LogAsync(data.RiskId, $"Validating if there are existing drivers.");
            if (!data.RiskDTO.Drivers.Any(x => x.DeletedDate is null))
            {
                errors.Add($"No driver available to rate for Endorsement {data.EndorsementNumber}.");
            }

            if (errors.Any())
            {
                throw new RaterException(errors);
            }
        }
    }
}
