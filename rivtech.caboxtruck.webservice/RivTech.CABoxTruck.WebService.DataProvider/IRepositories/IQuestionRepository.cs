using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IQuestionRepository : IRepository<Question, Int16>
    {
        //Task<IEnumerable<Question>> GetAllIncludeAsync();
        //Task<IEnumerable<Question>> GetAllIncludeAsync(Expression<Func<Question, bool>> expression);
    }
}
