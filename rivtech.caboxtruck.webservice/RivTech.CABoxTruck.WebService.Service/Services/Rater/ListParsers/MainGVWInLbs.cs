using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater.ListParsers
{
    public class MainGVWInLbs : IListParser
    {
        readonly List<dynamic> items = new List<dynamic>
        {
            new { Id = 1, Description = "0 - 10,000" },
            new { Id = 2, Description = "10,001 - 20,000" },
            new { Id = 3, Description = "20,001 - 45,000" },
            new { Id = 4, Description = "45,001 +" }
        };

        public Task<string> ParseAsync(object id, int? raterVersion = null)
        {
            var item = items.FirstOrDefault(x => x.Id == (short)id);
            return Task.FromResult(item?.Description ?? "");
        }
    }
}