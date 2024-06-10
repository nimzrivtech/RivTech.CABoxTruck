using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater.ListParsers
{
    public class MainGVWParser : IListParser
    {
        readonly List<dynamic> items = new List<dynamic>
        {
            new { Id = 1, Description = "Light" },
            new { Id = 2, Description = "Medium" },
            new { Id = 3, Description = "Heavy" },
            new { Id = 4, Description = "Extra Heavy" },
            new { Id = 5, Description = "Light Trucks" },
            new { Id = 6, Description = "Medium Trucks" },
            new { Id = 7, Description = "Heavy Trucks" },
            new { Id = 8, Description = "Extra Heavy Trucks" },
            new { Id = 9, Description = "Not a Truck/Tractor" },
            new { Id = 10, Description = "Extra Heavy Truck Tractors" },
            new { Id = 11, Description = "Heavy Truck Tractors" },
            new { Id = 12, Description = "Trailer Types" }
        };

        public Task<string> ParseAsync(object id,int? raterVersion = null)
        {
            var item = items.FirstOrDefault(x => x.Id == (short)id);
            return Task.FromResult(item?.Description ?? "");
        }


        
    }
}
