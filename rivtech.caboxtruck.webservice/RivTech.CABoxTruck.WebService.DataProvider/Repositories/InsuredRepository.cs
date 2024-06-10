using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class InsuredRepository : BaseRepository<Insured, Guid>, IInsuredRepository
    {
        public InsuredRepository(AppDbContext context)
           : base(context)
        {
        }

        public async Task<Insured> GetInsuredAsync(Guid id)
        {
            return await _context.Insured.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
