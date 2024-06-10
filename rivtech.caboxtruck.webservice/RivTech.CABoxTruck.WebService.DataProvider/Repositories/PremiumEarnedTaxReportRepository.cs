using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.DTO.Report;
using RivTech.CABoxTruck.WebService.DTO.Report.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public interface IPremiumEarnedTaxReportRepository
    {
        Task<IEnumerable<PremiumEarnedTaxReportDTO>> GetAsync(PremiumEarnedTaxReportRequestDTO request);
    }

    public class PremiumEarnedTaxReportRepository : IPremiumEarnedTaxReportRepository
    {
        private readonly AppDbContext _context;

        public PremiumEarnedTaxReportRepository(
            AppDbContext context
        )
        {
            _context = context;
        }

        public async Task<IEnumerable<PremiumEarnedTaxReportDTO>> GetAsync(PremiumEarnedTaxReportRequestDTO request)
        {
            using var conn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString);
            await conn.OpenAsync();
            try
            {
                request.ToDate = request.ToDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                var param = new DynamicParameters();
                param.Add("@fromDate", request.FromDate);
                param.Add("@toDate", request.ToDate);
                var result = await conn.QueryAsync<PremiumEarnedTaxReportDTO>(
                    "GeneratePremiumEarnedTaxReport @fromDate, @toDate",
                    param,
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
