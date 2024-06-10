using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IDriverRepository : IRepository<Driver, Guid>
    {
        Task<IEnumerable<Driver>> GetAllIncludeAsync();
        Task<IEnumerable<Driver>> GetAllIncludeAsync(Expression<Func<Driver, bool>> expression);
        Task<Driver> GetIncludeAsync(Expression<Func<Driver, bool>> expression);

        Driver GetById(Guid driverId);
        Task<List<Driver>> GetByRiskDetailAllAsync(Guid riskDetailId);
        Task<List<Driver>> GetByRiskDetailAsync(Guid riskDetailId);
        Task<List<Driver>> GetPreviousRiskDetailDrivers(Guid riskId);
        Task<IEnumerable<Driver>> GetDriversByLastNameAndLicenseNumberAsync(string lastName, string licenseNumber);
        Task<Driver> GetByRiskDetailAndIdAsync(Guid riskDetailId, Guid? driverId);
        Task<Driver> GetByIdAsync(Guid driverId);
    }

    public interface IDriverIncidentsRepository : IRepository<DriverIncidents, Guid>
    {
    }

    public interface IDriverHeaderRepository : IRepository<DriverHeader, Guid>
    {
    }
}
