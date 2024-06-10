using RivTech.CABoxTruck.WebService.DTO.Questions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IQuestionService
    {
        Task<QuestionDTO> GetQuestion(string description);
        //Task<List<QuestionDTO>> GetAllIncludeAsync();
        //Task<List<QuestionDTO>> GetAllByCategorySection(Int16 categoryId, Int16 sectionId);
    }
}
