using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO.Questions;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        public QuestionController(
            IQuestionService questionService
        )
        {
            _questionService = questionService;
        }

        //[HttpGet]
        //[Produces(typeof(List<QuestionDTO>))]
        //public async Task<IActionResult> GetQuestions()
        //{
        //    try
        //    {
        //        var res = await _questionService.GetAllIncludeAsync();
        //        return Ok(res);
        //    }
        //    catch (ModelNotFoundException)
        //    {
        //        return NotFound();
        //    }
        //}

        //[HttpGet("{categoryId}/{sectionId}")]
        //[Produces(typeof(QuestionDTO))]
        //public async Task<IActionResult> GetQuestionByCategorySection(Int16 categoryId, Int16 sectionId)
        //{
        //    try
        //    {
        //        var res = await _questionService.GetAllByCategorySection(categoryId, sectionId);
        //        return Ok(res);
        //    }
        //    catch (ModelNotFoundException)
        //    {
        //        return NotFound();
        //    }
        //}
    }
}