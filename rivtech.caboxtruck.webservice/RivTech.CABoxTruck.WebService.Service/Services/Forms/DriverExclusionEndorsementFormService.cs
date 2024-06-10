using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeComparer;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Common;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeComparers;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Forms
{
    public interface IDriverExclusionEndorsementFormService
    {
        Task Update(Guid riskDetailId);
    }

    public class DriverExclusionEndorsementFormService : IDriverExclusionEndorsementFormService
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IBrokerInfoRepository _brokerInfoRepository;
        private readonly IRiskFormRepository _riskFormRepository;
        private readonly IRiskRepository _riskRepository;
        private readonly IChangeComparerContext _changeComparerContext;
        private readonly IMapper _mapper;

        public DriverExclusionEndorsementFormService(
            IDriverRepository driverRepository,
            IRiskDetailRepository riskDetailRepository,
            IBrokerInfoRepository brokerInfoRepository,
            IRiskFormRepository riskFormRepository,
            IRiskRepository riskRepository,
            IChangeComparerContext changeComparerContext,
            IMapper mapper
        )
        {
            _driverRepository = driverRepository;
            _riskDetailRepository = riskDetailRepository;
            _brokerInfoRepository = brokerInfoRepository;
            _riskFormRepository = riskFormRepository;
            _riskRepository = riskRepository;
            _changeComparerContext = changeComparerContext;
            _mapper = mapper;
        }

        public async Task Update(Guid riskDetailId)
        {
            var riskDetail = await _riskDetailRepository.FindAsync(rd => rd.Id == riskDetailId);
            var latestRiskDetailForm = await _riskFormRepository.FindAsync(x => x.RiskDetailId == riskDetail.Id && x.FormId == "DriverExclusion");

            if (latestRiskDetailForm is null)
                return;

            var risk = await _riskRepository.FindAsync(riskDetail.RiskId);
            var submissionRiskDetail = await _riskDetailRepository.GetFirstRiskDetailByRiskIdAsync(risk.Id);
            var brokerInfo = await _brokerInfoRepository.FindAsync(x => x.RiskId == riskDetail.RiskId);
            var policyEffectiveDate = brokerInfo.EffectiveDate.Value;
            var drivers = await _driverRepository.GetByRiskDetailAllAsync(riskDetail.Id);
            var isS37KORulesUpdateApplicable = await IsFeatureApplicable(risk.Id, "S37KORulesUpdate");
            var formData = new FormDriverExclusionDTO();

            formData.Drivers = risk.IsIssued()
                ? await GetExcludedDriversForPolicy(drivers, policyEffectiveDate, submissionRiskDetail)
                : await GetExcludedDriversForSubmission(drivers, policyEffectiveDate, riskDetail);

            if (formData.Drivers.Any())
            {
                formData.UpdatedDate = riskDetail.EndorsementEffectiveDate ?? policyEffectiveDate;
                formData.SortDrivers();
                latestRiskDetailForm.IncludeWithData(JsonConvert.SerializeObject(formData));

                if(!isS37KORulesUpdateApplicable)
                    latestRiskDetailForm.IsSelected ??= true;
            }
            else
            {
                latestRiskDetailForm?.Exclude();
            }

            _riskFormRepository.Update(latestRiskDetailForm);
            await _riskFormRepository.SaveChangesAsync();
        }

        private async Task<List<FormDriverDTO>> GetExcludedDriversForPolicy(List<Driver> drivers, DateTime policyEffectiveDate, RiskDetail submissionRiskDetail)
        {
            var risk = await _riskRepository.FindAsync(submissionRiskDetail.RiskId);
            var fieldVersions = await _riskRepository.GetFieldVersions();
            bool isDriverKO = await IsFeatureApplicable(risk.Id, "S37KORulesUpdate");

            var excludedDrivers = drivers.Select(x => _mapper.Map<DriverDTO>(x))
                                .Where(driver => IsDriverInKoRule(driver, policyEffectiveDate, isDriverKO)
                                    || driver.DeletedDate != null);
            var excludedFormDrivers = ToFormDriverDTO(excludedDrivers);

            var submissionDriverExclusionForm = await _riskFormRepository.FindAsync(x => x.RiskDetailId == submissionRiskDetail.Id && x.FormId == "DriverExclusion");
            if (submissionDriverExclusionForm.Other == null)
                return excludedFormDrivers;

            var submissionDriverExclusionFormData = JsonConvert.DeserializeObject<FormDriverExclusionDTO>(submissionDriverExclusionForm.Other);
            if (!submissionDriverExclusionFormData.Drivers?.Any() ?? false)
                return excludedFormDrivers;

            // Re-add drivers excluded from submission.
            foreach (var driver in submissionDriverExclusionFormData.Drivers)
            {
                bool isExisting = excludedDrivers.Any(x => x.LicenseNumber == driver.LicenseNumber
                                && x.FirstName == driver.FirstName
                                && x.LastName == driver.LastName);
                if (isExisting)
                    continue;

                excludedFormDrivers.Add(driver);
            }

            return excludedFormDrivers;
        }

        private async Task<List<FormDriverDTO>> GetExcludedDriversForSubmission(List<Driver> drivers, DateTime policyEffectiveDate, RiskDetail riskDetail)
        {
            var risk = await _riskRepository.FindAsync(riskDetail.RiskId);
            var fieldVersions = await _riskRepository.GetFieldVersions();
            bool isDriverKO = await IsFeatureApplicable(risk.Id, "S37KORulesUpdate");

            var excludedDrivers = drivers.Select(x => _mapper.Map<DriverDTO>(x))
                                .Where(driver => IsDriverInKoRule(driver, policyEffectiveDate, isDriverKO));
            var excludedFormDrivers = ToFormDriverDTO(excludedDrivers);
            return await Task.FromResult(excludedFormDrivers);
        }


        private List<FormDriverDTO> ToFormDriverDTO(IEnumerable<DriverDTO> drivers)
        {
            return drivers.Select(x => new FormDriverDTO
            {
                LicenseNumber = x.LicenseNumber,
                FirstName = x.FirstName,
                LastName = x.LastName
            }).ToList();
        }

        public bool IsDriverInKoRule(DriverDTO driver, DateTime policyEffectiveDate, bool isNewImplem = false)
        {
            var endDate = policyEffectiveDate.Date;
            var prev3years = endDate.AddMonths(-36).Date;
            var prev5years = endDate.AddMonths(-60).Date;
            var majorIncidents = new[] { "1", "2", "5" };
            var minorIncidents = new[] { "3", "4", "6", "7", "8", "9" };
            var movingViolations = new[] { "1", "2", "4", "5", "8" };

            if (driver.isExcludeKO == true)
                return false;

            if (isNewImplem)
            {
                var isAgeStringValid = int.TryParse(driver.Age, out var driverAge);
                if (!isAgeStringValid) return false;

                var minorIncidentCount = driver.DriverIncidents.Count(di =>
                    minorIncidents.Contains(di.IncidentType)
                    && di.IncidentDate >= prev3years
                    && di.IncidentDate <= endDate
                    && driver.isExcludeKO == false);

                var majorIncidentCount = driver.DriverIncidents.Count(di =>
                    majorIncidents.Contains(di.IncidentType)
                    && di.IncidentDate >= prev5years
                    && di.IncidentDate <= endDate
                    && driver.isExcludeKO == false);

                if ((driverAge == 24 || driverAge == 25) && minorIncidentCount >= 1) return true;
                if (driverAge <= 22) return true;
                if (minorIncidentCount >= 3) return true;
                if (majorIncidentCount >= 1) return true;
                if (driver.YrsCommDrivingExp < 2) return true;
                if (driverAge > 70) return true;

                return false;
            }
            else
            {
                var isAgeStringValid = int.TryParse(driver.Age, out var driverAge);
                if (!isAgeStringValid) return false;
                if (driverAge <= 22) return true;
                if ((driverAge == 23 || driverAge == 24) && driver.IsMvr == false && driver.isExcludeKO == false) return true;

                var movingViolationsCount = driver.DriverIncidents.Count(di =>
                    movingViolations.Contains(di.IncidentType)
                    && di.IncidentDate <= endDate
                    && di.IncidentDate >= prev3years
                    && driver.isExcludeKO == false);

                if (movingViolationsCount > 1) return true;
            }

            return false;
        }

        private async Task<bool> IsFeatureApplicable(Guid RiskId, string fieldName)
        {
            var fieldVersions = await _riskRepository.GetFieldVersions();
            var submissionRiskDetail = await _riskDetailRepository.GetFirstRiskDetailByRiskIdAsync(RiskId);
            var risk = await _riskRepository.FindAsync(RiskId);
            var isApplicable = fieldVersions.Any(v => v.FieldName == fieldName && submissionRiskDetail.RequestedQuoteDate >= v.EffectiveDate);

            if (submissionRiskDetail.RequestedQuoteDate == null && !risk.IsRaterVersionLocked)
                return fieldVersions.Any(v => v.FieldName == fieldName);

            return isApplicable;
        }
    }
}
