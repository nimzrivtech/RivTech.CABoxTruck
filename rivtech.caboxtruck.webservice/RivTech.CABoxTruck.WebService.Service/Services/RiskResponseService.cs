using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.Data.Entity;
using System.Reflection;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class RiskResponseService : IRiskResponseService
    {
        private readonly IRiskResponseRepository _riskResponseRepository;
        private readonly IQuestionService _questionService;
        private readonly IRiskSpecificCommoditiesHauledRepository _commoditiesHauledRepository;
        private readonly IMapper _mapper;

        public RiskResponseService(IRiskResponseRepository riskResponseRepository,
            IQuestionService questionService,
            IRiskSpecificCommoditiesHauledRepository commoditiesHauledRepository,
            IMapper mapper)
        {
            _riskResponseRepository = riskResponseRepository;
            _questionService = questionService;
            _commoditiesHauledRepository = commoditiesHauledRepository;
            _mapper = mapper;
        }

        public async Task<List<RiskResponseDTO>> GetRiskResponse(Guid riskId)
        {
            var safetyDevice = await _riskResponseRepository.GetAsync(x => x.RiskDetailId == riskId);
            return _mapper.Map<List<RiskResponseDTO>>(safetyDevice);
        }

        public async Task<List<RiskResponseDTO>> GetRiskResponseAllInclude(Guid riskId, Int16 sectionId)
        {
            var riskResponse = await _riskResponseRepository.GetAsync(r => r.RiskDetailId == riskId && r.Question.SectionId == sectionId, r => r.Question);
            return _mapper.Map<List<RiskResponseDTO>>(riskResponse);
        }

        public async Task<RiskResponseDTO> InsertRiskResponse(RiskResponseDTO safetyDevice)
        {
            var result = await _riskResponseRepository.AddAsync(_mapper.Map<RiskResponse>(safetyDevice));
            await _riskResponseRepository.SaveChangesAsync();
            return _mapper.Map<RiskResponseDTO>(result);
        }

        public async Task<RiskResponseDTO> UpdateSpecificSafetyDevice(RiskResponseDTO safetyDevice, Guid riskId)
        {
            var result = _riskResponseRepository.Update(_mapper.Map<RiskResponse>(safetyDevice));
            await _riskResponseRepository.SaveChangesAsync();
            return _mapper.Map<RiskResponseDTO>(result);
        }

        public async Task<List<RiskResponseDTO>> SaveRiskResponse(List<RiskResponseDTO> riskResponses, Int16 sectionId, Guid riskId)
        {
            List<RiskResponseDTO> riskResponseResults = new List<RiskResponseDTO>();
            var existingRiskResponse = await _riskResponseRepository.GetAsync(x => x.RiskDetailId == riskId && x.Question.SectionId == sectionId, x => x.Question);

            foreach (var riskResponse in riskResponses)
            {
                riskResponse.RiskDetailId = riskId;
                var existingData = existingRiskResponse.FirstOrDefault(x => x.QuestionId == riskResponse.QuestionId);

                if (existingData == null)
                {
                    riskResponseResults.Add(await InsertRiskResponse(riskResponse));
                }
                else
                {
                    riskResponse.Id = existingData.Id;
                    riskResponseResults.Add(await UpdateSpecificSafetyDevice(riskResponse, riskId));
                }
            }

            return riskResponseResults;
        }

        public async Task<bool> Delete(Guid riskId, List<int> sectionIds)
        {
            foreach (var sectionId in sectionIds)
            {
                var responsesForDelete = await _riskResponseRepository.GetAsync(x => x.RiskDetailId == riskId && x.Question.SectionId == sectionId);
                if (sectionId == LvQuestionSection.Cargo.Id)
                {
                    var commoditiesHauled = await _commoditiesHauledRepository.GetAsync(c => c.RiskDetailId == riskId);
                    _commoditiesHauledRepository.RemoveRange(commoditiesHauled);
                    await _commoditiesHauledRepository.SaveChangesAsync();
                }
                _riskResponseRepository.RemoveRange(responsesForDelete);
            }

            await _riskResponseRepository.SaveChangesAsync();
            return true;
        }

        public async Task<List<RiskResponseDTO>> MapToRiskResponse<TEntity>(TEntity entity, Guid riskId)
        {
            var riskResponse = new List<RiskResponseDTO>();
            var properties = entity.GetType().GetProperties();

            foreach (var prop in properties)
            {
                var question = await _questionService.GetQuestion(prop.Name);
                var value = prop.GetValue(entity, null);
                if (question == null) continue;

                var valueStr = prop.PropertyType == typeof(List<SelectItemDTO>) ? GetSelectItemValue(value as List<SelectItemDTO>) : value?.ToString();

                riskResponse.Add(new RiskResponseDTO()
                {
                    QuestionId = question.Id,
                    RiskDetailId = riskId,
                    ResponseValue = valueStr,
                });
            }
            return riskResponse;
        }

        private static string GetSelectItemValue(IReadOnlyCollection<SelectItemDTO> selectItems)
        {
            return selectItems == null ? null : string.Join(',', selectItems.Select(i => i.Value).ToArray());
        }

        public TEntity MapToEntityType<TEntity>(TEntity entity, List<RiskResponseDTO> riskResponses)
        {
            foreach (var response in riskResponses)
            {
                var question = response.Question;
                
                var property = entity.GetType().GetProperty(question.Description, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                var propertyType = Nullable.GetUnderlyingType(property.PropertyType);
                if (propertyType == null) propertyType = property.PropertyType;

                if (response.ResponseValue == null) continue;

                if (propertyType == typeof(string)) property.SetValue(entity, response.ResponseValue);
                else if (propertyType == typeof(List<SelectItemDTO>)) property.SetValue(entity, GetDropdownValue(response.ResponseValue, property.Name));
                else property.SetValue(entity, Convert.ChangeType(response.ResponseValue, propertyType));
            }

            return entity;
        }

        private static List<SelectItemDTO> GetDropdownValue(string value, string name)
        {
            var selectItems = new List<SelectItemDTO>();
            if (string.IsNullOrWhiteSpace(value)) return selectItems;
            var values = value.Split(',');
            selectItems.AddRange(values.Select(val => new SelectItemDTO() {Value = Convert.ToInt16(val)}));
            return selectItems;
        }

    }
}


