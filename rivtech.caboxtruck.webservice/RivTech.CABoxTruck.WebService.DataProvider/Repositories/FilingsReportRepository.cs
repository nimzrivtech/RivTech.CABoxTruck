using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO.Report;
using RivTech.CABoxTruck.WebService.DTO.Report.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class FilingsReportRepository : IFilingsReportRepository
    {
        private readonly AppDbContext _context;

        public FilingsReportRepository(
            AppDbContext context
        )
        {
            _context = context;
        }

        public async Task<IEnumerable<FilingsReportDTO>> GetAsync()
        {
            using var conn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString);
            await conn.OpenAsync();
            try
            {
                var result = await conn.QueryAsync<FilingsReportDTO>(
                    "GenerateFilingsReport",
                    null,
                    commandType: CommandType.Text,
                    commandTimeout: 300);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
