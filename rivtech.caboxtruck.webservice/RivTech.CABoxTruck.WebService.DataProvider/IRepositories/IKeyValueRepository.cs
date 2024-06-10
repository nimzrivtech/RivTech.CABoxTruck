using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IKeyValueRepository
    {
        IEnumerable<TEntity> Retrieve<TEntity>() where TEntity : class;
        TEntity RetrieveByName<TEntity>(string name) where TEntity : class;
        TEntity RetrieveById<TEntity>(string id) where TEntity : class;
    }
}