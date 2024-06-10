using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater.ListParsers
{
    public class RatingTypeParser : IListParser
    {
        readonly List<dynamic> items = new List<dynamic>
        {
            new { Id = 1, Description = "Garaging Terr Only" },
            new { Id = 2, Description = "Primary Terr Only" },
            new { Id = 3, Description = "Weighted Average" },
            new { Id = 4, Description = "Worst Area" },
            new { Id = 5, Description = "Other" }
        };

        public Task<string> ParseAsync(object id, int? raterVersion = null)
        {
            var item = items.FirstOrDefault(x => x.Id == (short)id);
            return Task.FromResult(item?.Description ?? "");
        }
    }
}
