using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class LvDatasetRepository : BaseRepository<LvDataset, int>, ILvDatasetRepository
    {
        public LvDatasetRepository(AppDbContext context)
          : base(context)
        {
        }
    }
}
