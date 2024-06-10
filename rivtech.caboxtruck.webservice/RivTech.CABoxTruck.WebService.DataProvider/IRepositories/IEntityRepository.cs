using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IEntityRepository : IRepository<Entity, Guid>
    {
        Task<Entity> GetEntitiesAsync(Guid id);
    }
}
