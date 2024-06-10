using AutoMapper;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO.Questions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        public QuestionService(IQuestionRepository questionRepository, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public async Task<QuestionDTO> GetQuestion(string description)
        {
            var result = await _questionRepository.FindAsync(x => x.Description == description);
            return _mapper.Map<QuestionDTO>(result);
        }

        //public async Task<List<QuestionDTO>> GetAllIncludeAsync()
        //{
        //    var questions = await _questionRepository.GetAllIncludeAsync();
        //    return _mapper.Map<List<QuestionDTO>>(questions);
        //}

        //public async Task<List<QuestionDTO>> GetAllByCategorySection(Int16 categoryId, Int16 sectionId)
        //{
        //    var questions = await _questionRepository.GetAllIncludeAsync(q => q.CategoryId == categoryId && q.SectionId == sectionId);
        //    return _mapper.Map<List<QuestionDTO>>(questions);
        //}
    }
}
