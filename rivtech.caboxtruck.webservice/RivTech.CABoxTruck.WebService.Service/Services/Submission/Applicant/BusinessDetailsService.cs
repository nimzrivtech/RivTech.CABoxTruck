using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Submission.Applicant
{
    public class BusinessDetailsService : IBusinessDetailsService
    {
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IEntitySubsidiaryRepository _entitySubsidiaryRepository;
        private readonly IEntityRepository _entityRepository;
        private readonly IRiskStatusHistoryService _riskStatusHistoryService;
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;

        public BusinessDetailsService(IRiskDetailRepository riskDetailRepository, 
            IEntitySubsidiaryRepository entitySubsidiaryRepository,
            IRiskStatusHistoryService riskStatusHistoryService,
            IVehicleService vehicleService,
            IEntityRepository entityRepository, 
            IMapper mapper)
        {
            _riskDetailRepository = riskDetailRepository;
            _entitySubsidiaryRepository = entitySubsidiaryRepository;
            _entityRepository = entityRepository;
            _riskStatusHistoryService = riskStatusHistoryService;
            _vehicleService = vehicleService;
            _mapper = mapper;
        }

        public async Task<BusinessDetailsDTO> GetBusinessDetails(Guid id)
        {
            var risk = await _riskDetailRepository.GetInsuredAsync(id);

            var businessDetails = await _entityRepository.FindAsync(risk.Insured.EntityId);

            var subsidiaries = await _entitySubsidiaryRepository.GetAsync(x => x.EntityId == risk.Insured.EntityId && x.IsSubsidiaryCompany == true);
            var companies = await _entitySubsidiaryRepository.GetAsync(x => x.EntityId == risk.Insured.EntityId && x.IsCurrentlyOwned == true);

            return new BusinessDetailsDTO()
            {
                IsNewVenture = risk.IsNewVenture ?? false,
                FirstYearUnderCurrentManagement = risk.FirstYearUnderCurrentManagement,
                NewVenturePreviousExperienceId = risk.NewVenturePreviousExperienceId,
                YearsInBusiness = risk.YearsInBusiness,
                YearsUnderCurrentManagement = risk.YearsUnderCurrentManagement,
                MainUseId = risk.MainUseId,
                MainGVWId = risk.MainGVWId,
                AccountCategoryId = risk.AccountCategoryId,
                AccountCategoryUWId = risk.AccountCategoryUWId,
                SecondaryBusinessGroupId = risk.SecondaryBusinessGroupId,

                YearEstablished = businessDetails.YearEstablished,
                BusinessTypeId = businessDetails.BusinessTypeID,
                DescOperations = businessDetails.DescOperations,
                FederalIDNumber = businessDetails.FederalIDNumber,
                SocialSecurityNumber = businessDetails.SocialSecurityNumber,
                NAICSCode = businessDetails.NAICSCode,
                PUCNumber = businessDetails.PUCNumber,
                HasSubsidiaries = businessDetails.HasSubsidiaries,
                HasCompanies = businessDetails.HasCompanies,

                Subsidiaries = _mapper.Map<List<EntitySubsidiaryDTO>>(subsidiaries),
                Companies = _mapper.Map<List<EntitySubsidiaryDTO>>(companies),
            };
        }

        public async Task<BusinessDetailsDTO> InsertUpdateBusinessDetailsAsync(BusinessDetailsDTO model)
        {
            var risk = await _riskDetailRepository.GetInsuredAsync(model.RiskDetailId);

            risk.IsNewVenture = model.IsNewVenture;
            risk.FirstYearUnderCurrentManagement = model.FirstYearUnderCurrentManagement;
            risk.NewVenturePreviousExperienceId = model.NewVenturePreviousExperienceId;
            risk.YearsInBusiness = model.YearsInBusiness;
            risk.YearsUnderCurrentManagement = model.YearsUnderCurrentManagement;
            risk.MainUseId = model.MainUseId;
            risk.MainGVWId = model.MainGVWId == 0 ? null : model.MainGVWId;
            risk.AccountCategoryId = model.AccountCategoryId;
            risk.AccountCategoryUWId = model.AccountCategoryUWId;
            risk.SecondaryBusinessGroupId = model.SecondaryBusinessGroupId == 0 ? null : model.SecondaryBusinessGroupId;
            risk.Insured.Entity.BusinessTypeID = model.BusinessTypeId;
            risk.Insured.Entity.YearEstablished = model.YearEstablished;
            risk.Insured.Entity.FederalIDNumber = model.FederalIDNumber.NullIfEmpty();
            risk.Insured.Entity.NAICSCode = model.NAICSCode.NullIfEmpty();
            risk.Insured.Entity.PUCNumber = model.PUCNumber.NullIfEmpty();
            risk.Insured.Entity.SocialSecurityNumber = model.SocialSecurityNumber.NullIfEmpty();
            risk.Insured.Entity.HasSubsidiaries = model.HasSubsidiaries;
            risk.Insured.Entity.HasCompanies = model.HasCompanies;
            risk.Insured.Entity.DescOperations = model.DescOperations.NullIfEmpty();

            if (risk.Status == RiskDetailStatus.Received)
            {
                await _riskStatusHistoryService.InsertAsync(new RiskStatusHistory
                {
                    RiskDetailId = risk.Id,
                    ChangedBy = (int)Data.Common.Services.GetCurrentUser(),
                    ChangedDate = DateTime.Now,
                    ChangedField = "Status",
                    OldValue = risk.Status,
                    NewValue = RiskDetailStatus.InResearch
                });

                risk.Status = RiskDetailStatus.InResearch;
            }

            _riskDetailRepository.Update(risk);
            await _riskDetailRepository.SaveChangesAsync();


            //#region Update vehicles Main Use and Account category

            //if (!(risk.Status == RiskDetailStatus.Active || risk.Status == RiskDetailStatus.Canceled))
            //{
            //    if (model.MainUseId.HasValue)
            //        await _vehicleService.UpdateMainUseAsync(model.RiskDetailId, model.MainUseId.Value);

            //    if (model.AccountCategoryId.HasValue)
            //        await _vehicleService.UpdateAccountCategoryAsync(model.RiskDetailId, model.AccountCategoryUWId.Value);
            //}
            //#endregion
            
            return model;
        }
    }
}
