using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class BrokerCommissionRepository : IBrokerCommissionRepository
    {
        private readonly AppDbContext _context;
        public BrokerCommissionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BrokerCommissionDTO>> GenerateBrokerCommissionAsync(BrokerCommissionInputDTO data)
        {
            using (SqlConnection conn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                await conn.OpenAsync();
                try
                {
                    data.ToDate = data.ToDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                    var param = new DynamicParameters();
                    param.Add("@fromDate", data.FromDate);
                    param.Add("@toDate", data.ToDate);
                    var result = await conn.QueryAsync<BrokerCommissionDTO>("GenerateBrokerCommission @fromDate, @toDate",
                                                                            param, commandType: CommandType.Text, commandTimeout: 300
                                                                            );
                    
                    foreach(var item in result)
                    {
                        if (item.VoidDate != null)
                        {
                            item.CheckNumber += " (void)";
                        }
                    }

                    return result.Where(x => x.CommisionALAmount != null || x.CommisionPDAmount != null).OrderBy(x => x.CompanyName).ToList();
                }
                catch (System.Exception)
                {
                    throw;
                }
            }
        }

        
    }
}


