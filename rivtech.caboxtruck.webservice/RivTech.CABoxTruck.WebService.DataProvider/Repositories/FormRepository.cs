using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class FormRepository : BaseRepository<Form, string>, IFormRepository
    {
        public FormRepository(AppDbContext context)
            : base(context)
        {
        }
    }
}