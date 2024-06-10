using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories.Dashboard
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _context;

        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<dynamic> GetDashboardCount()
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            if (command.Connection.State != ConnectionState.Open)
                command.Connection.Open();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "GetDashboardCount";

            using var reader = command.ExecuteReader();

            var names = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
            foreach (IDataRecord record in reader as IEnumerable)
            {
                var expando = new ExpandoObject() as IDictionary<string, object>;
                foreach (var name in names)
                    expando[name] = record[name];

                yield return expando;
            }
        }
    }
}
