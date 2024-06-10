using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class RiskFormRepository : BaseRepository<RiskForm, Guid>, IRiskFormRepository
    {
        public RiskFormRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task UpdateRiskForm(RiskForm form)
        {
            var riskForm = await _context.Set<RiskForm>().Where(x => x.Id == form.Id && x.RiskDetailId == form.RiskDetailId).FirstOrDefaultAsync();

            if (riskForm != null)
            {
                riskForm.Other = form.Other;
                /*** other details here ***/
                _context.Update(riskForm);
            }
        }

        public async Task<List<RiskForm>> GetByIdIncludeAsync(Guid id)
        {
            var result = await _context.Set<RiskForm>()
                .Include(x => x.Form.FormVersions)
                .Where(d => d.RiskDetailId == id)
                .ToListAsync();

            return result;
        }
    }
}