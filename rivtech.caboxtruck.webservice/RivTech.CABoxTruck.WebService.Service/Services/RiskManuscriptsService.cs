using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Submission.Applicant;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class RiskManuscriptsService : IRiskManuscriptsService
    {
        private readonly IRiskManuscriptsRepository _manuscriptsRepository;
        private readonly IQuoteOptionsService _quoteOptionsService;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IMapper _mapper;

        public RiskManuscriptsService(IRiskManuscriptsRepository manuscriptsRepository,
            IQuoteOptionsService quoteOptionsService,
            IRiskDetailRepository riskDetailRepository,
            IMapper mapper)
        {
            _manuscriptsRepository = manuscriptsRepository;
            _quoteOptionsService = quoteOptionsService;
            _riskDetailRepository = riskDetailRepository;
            _mapper = mapper;
        }

        public async Task<List<RiskManuscriptsDTO>> GetByRiskIdAsync(Guid riskId)
        {
            var result = await _manuscriptsRepository.GetAsync(rf => rf.RiskDetailId == riskId);
            return _mapper.Map<List<RiskManuscriptsDTO>>(result);
        }

        public async Task<List<RiskManuscriptsDTO>> GetByRiskIdAllAsync(Guid riskDetailId)
        {
            var returnList = new List<RiskManuscriptsDTO>();
            var result = await _manuscriptsRepository.GetAsync(rf => rf.RiskDetailId == riskDetailId);
            // Previous RiskDetail
            var currentRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            var previousRiskDetails = await _riskDetailRepository.GetAsync(riskDetail => riskDetail.RiskId == currentRiskDetail.RiskId && riskDetail.Id != currentRiskDetail.Id);
            var previousRiskDetailId = previousRiskDetails.OrderByDescending(riskDetail => riskDetail.UpdatedDate).Select(riskDetail => riskDetail.Id).First();
            var previousRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(previousRiskDetailId);
            var previousManuscripts = await _manuscriptsRepository.GetAsync(x => x.RiskDetailId == previousRiskDetail.Id);
            foreach (var item in result)
            {
                var manuscript = _mapper.Map<RiskManuscriptsDTO>(item);
                var isNewManuscript = previousManuscripts.All(oldObj => oldObj.CreatedDate != item.CreatedDate);
                if (!isNewManuscript)
                    manuscript.fromPreviousEndorsement = true;

                returnList.Add(manuscript);
            }

            return returnList;
        }

        public async Task<RiskManuscriptsDTO> InsertAsync(RiskManuscriptsDTO resource)
        {
            var model = _mapper.Map<RiskManuscripts>(resource);
            model.IsActive = true;
            model.PremiumType = "AL";
            model.IsSelected = true;
            model.ProratedPremium = model.Premium;
            var result = await _manuscriptsRepository.AddAsync(model);
            await _quoteOptionsService.UpdateManuscript(_mapper.Map<RiskManuscriptsDTO>(model)); // update premiums
            await _manuscriptsRepository.SaveChangesAsync();
            return _mapper.Map<RiskManuscriptsDTO>(result);
        }

        public async Task<RiskManuscriptsDTO> UpdateAsync(RiskManuscriptsDTO resource)
        {
            var model = await _manuscriptsRepository.FindAsync(x => x.Id == Guid.Parse(resource.Id));

            // Previous RiskDetail
            var currentRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(model.RiskDetailId);
            var previousRiskDetails = await _riskDetailRepository.GetAsync(riskDetail => riskDetail.RiskId == currentRiskDetail.RiskId && riskDetail.Id != currentRiskDetail.Id);
            var previousManuscripts = new List<RiskManuscripts>();
            if (previousRiskDetails.ToList().Count > 0)
            {
                var previousRiskDetailId = previousRiskDetails.OrderByDescending(riskDetail => riskDetail.UpdatedDate).Select(riskDetail => riskDetail.Id).FirstOrDefault();
                var previousRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(previousRiskDetailId);
                previousManuscripts = (await _manuscriptsRepository.GetAsync(x => x.RiskDetailId == previousRiskDetail.Id)).ToList();
            }

            //Manuscript is selected/unselected from Forms Selection
            if (model.IsSelected != resource.IsSelected)
            {
                await _quoteOptionsService.UpdateManuscript(resource);
            }
            //Manuscript is updated from Manuscripts
            else
            {
                if (model.IsSelected.GetValueOrDefault() && (model.Premium != resource.Premium || model.PremiumType != resource.PremiumType))
                {
                    model.IsSelected = false;
                    await _quoteOptionsService.UpdateManuscript(_mapper.Map<RiskManuscriptsDTO>(model));
                    await _quoteOptionsService.UpdateManuscript(resource);
                }
            }

            model.Title = resource.Title;
            model.Description = resource.Description;
            model.Premium = resource.Premium;
            model.ProratedPremium = resource.Premium;
            model.IsProrate = resource.IsProrate;
            model.PremiumType = resource.PremiumType;
            model.IsSelected = resource.IsSelected;
            var result = _manuscriptsRepository.Update(model);

            await _manuscriptsRepository.SaveChangesAsync();

            var manuscript = _mapper.Map<RiskManuscriptsDTO>(result);
            if (previousManuscripts.Count > 0)
            {
                var isNewManuscript = previousManuscripts.All(oldObj => oldObj.CreatedDate != result.CreatedDate);
                if (!isNewManuscript)
                    manuscript.fromPreviousEndorsement = true;
            }

            return manuscript;
        }

        public async Task<bool> UpdateDatesAsync(RiskManuscriptsDTO resource)
        {
            var model = await _manuscriptsRepository.GetAsync(x => x.RiskDetailId == resource.RiskDetailId);
            foreach (var manuscript in model)
            {
                manuscript.EffectiveDate = resource.EffectiveDate;
                manuscript.ExpirationDate = resource.ExpirationDate;
                _manuscriptsRepository.Update(manuscript);
            }
            await _manuscriptsRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(Guid id, bool fromEndorsement, DateTime expirationDate)
        {
            var model = await _manuscriptsRepository.FindAsync(m => m.Id == id);

            if (model.IsSelected.GetValueOrDefault())
            {
                model.IsSelected = false;
                var manuscriptDto = _mapper.Map<RiskManuscriptsDTO>(model);
                await _quoteOptionsService.UpdateManuscript(manuscriptDto);
            }

            if (!fromEndorsement)
            {
                // Hard Delete if Submission
                _manuscriptsRepository.Remove(model);
                await _manuscriptsRepository.SaveChangesAsync();
                return true;
            }

            // Previous RiskDetail
            var currentRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(model.RiskDetailId);
            var previousRiskDetails = await _riskDetailRepository.GetAsync(riskDetail => riskDetail.RiskId == currentRiskDetail.RiskId && riskDetail.Id != currentRiskDetail.Id);
            var previousRiskDetailId = previousRiskDetails.OrderByDescending(riskDetail => riskDetail.UpdatedDate).Select(riskDetail => riskDetail.Id).First();
            var previousRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(previousRiskDetailId);
            var previousManuscripts = await _manuscriptsRepository.GetAsync(x => x.RiskDetailId == previousRiskDetail.Id);

            var isNewManuscript = previousManuscripts.All(oldObj => oldObj.CreatedDate != model.CreatedDate);
            if (isNewManuscript)
            {
                // Hard Delete if Endorsement new added manuscript
                _manuscriptsRepository.Remove(model);
                await _manuscriptsRepository.SaveChangesAsync();
                return true;
            }

            model.IsActive = false;
            if (model.IsProrate) { model.ExpirationDate = expirationDate; }
            else { model.ProratedPremium = 0; } // set prorated amount = 0 for fully earned manuscript to be exempted in premium change table
            model.DeletedDate = DateTime.Now;
            _manuscriptsRepository.Update(model);
            await _manuscriptsRepository.SaveChangesAsync();
            return false;
        }

        public async Task<RiskManuscriptsDTO> InsertManuscriptAsync(RiskManuscriptsDTO resource)
        {
            var model = _mapper.Map<RiskManuscripts>(resource);
            var result = await _manuscriptsRepository.AddAsync(model);
            await _quoteOptionsService.UpdateManuscript(_mapper.Map<RiskManuscriptsDTO>(model)); // update premiums
            await _manuscriptsRepository.SaveChangesAsync();
            return _mapper.Map<RiskManuscriptsDTO>(result);
        }
        public async Task<ManuscriptPagingDTO> GetByRiskIdAllAsync(ManuscriptFilterDTO manuscriptFilter)
        {

            var returnList = new List<RiskManuscriptsDTO>();
            IEnumerable<RiskManuscripts> previousManuscripts = null;


            Expression<Func<RiskManuscripts, bool>> expression = x => x.RiskDetailId == manuscriptFilter.RiskDetailId;

            #region Sorting
            Expression<Func<RiskManuscripts, object>> selector;
            selector = x => x.CreatedDate;

            if (manuscriptFilter.SortField == "title")
            {
                selector = x => x.Title;
            }

            else if (manuscriptFilter.SortField == "description")
            {
                selector = x => x.Description;
            }

            else if (manuscriptFilter.SortField == "premium")
            {
                selector = x => x.Premium;
            }
            else if (manuscriptFilter.SortField == "effectiveDate")
            {
                selector = x => x.EffectiveDate;
            }
            else if (manuscriptFilter.SortField == "expirationDate")
            {
                selector = x => x.ExpirationDate;
            }
            else if (manuscriptFilter.SortField == "premiumType")
            {
                selector = x => x.PremiumType;
            }
            else if (manuscriptFilter.SortField == "CreatedDate")
            {
                selector = x => x.CreatedDate;
            }
           

            #endregion

            var (result, paginationDetails) = await _manuscriptsRepository.GetWithPagingAsync(expression, selector, manuscriptFilter.PageNumber, manuscriptFilter.PageSize, manuscriptFilter.Order);

            // Previous RiskDetail
            if (manuscriptFilter.isPolicy)
            {
                var currentRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(manuscriptFilter.RiskDetailId.Value);
                var previousRiskDetails = await _riskDetailRepository.GetAsync(riskDetail => riskDetail.RiskId == currentRiskDetail.RiskId && riskDetail.Id != currentRiskDetail.Id);
                var previousRiskDetailId = previousRiskDetails.OrderByDescending(riskDetail => riskDetail.UpdatedDate).Select(riskDetail => riskDetail.Id).First();
                var previousRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(previousRiskDetailId);
                previousManuscripts = await _manuscriptsRepository.GetAsync(x => x.RiskDetailId == previousRiskDetail.Id);
            }
            
            foreach (var item in result)
            {
                var manuscript = _mapper.Map<RiskManuscriptsDTO>(item);

                if (manuscriptFilter.isPolicy)
                {
                    var isNewManuscript = previousManuscripts.All(oldObj => oldObj.CreatedDate != item.CreatedDate);
                    if (!isNewManuscript)
                        manuscript.fromPreviousEndorsement = true;
                }
               

                returnList.Add(manuscript);
            }

            return new ManuscriptPagingDTO()
            {
                PaginationDetails = paginationDetails,
                RiskManuscript = returnList

            };
        }
    }
}