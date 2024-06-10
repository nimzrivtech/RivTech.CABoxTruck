using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class QuestionRepository : BaseRepository<Question, Int16>, IQuestionRepository
    {
        public QuestionRepository(AppDbContext context)
           : base(context)
        {
        }

        //public async Task<IEnumerable<Question>> GetAllIncludeAsync()
        //{
        //    return await _context.Set<Question>()
        //        .Include(q => q.QuestionConfig)
        //        .Include(q => q.QuestionValidations).ThenInclude(qv => qv.Validation)
        //        .ToListAsync();
        //}

        //public async Task<IEnumerable<Question>> GetAllIncludeAsync(Expression<Func<Question, bool>> expression)
        //{
        //    return await _context.Set<Question>()
        //        .Include(q => q.QuestionConfig)
        //        .Include(q => q.QuestionValidations).ThenInclude(qv => qv.Validation)
        //        .Where(expression)
        //        .ToListAsync();
        //}
    }
}
