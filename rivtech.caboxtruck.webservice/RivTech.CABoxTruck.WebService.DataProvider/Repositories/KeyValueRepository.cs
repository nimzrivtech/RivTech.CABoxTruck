using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class KeyValueRepository : IKeyValueRepository
    {
        private readonly AppDbContext _context;

        public KeyValueRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TEntity> Retrieve<TEntity>() where TEntity : class
        {
            var result = _context.Set<TEntity>().AsNoTracking().ToList();
            return result;
        }

        //public async Task<IEnumerable<RiskStatus>> Retrieve()
        //{
        //    return await _context.Set<RiskStatus>().AsNoTracking().ToListAsync();
        //}

        public TEntity RetrieveByName<TEntity>(string name) where TEntity : class
        { 
            throw new NotImplementedException();
        }

        public TEntity RetrieveById<TEntity>(string id) where TEntity : class
        {
            throw new NotImplementedException();
        }
    }
}
