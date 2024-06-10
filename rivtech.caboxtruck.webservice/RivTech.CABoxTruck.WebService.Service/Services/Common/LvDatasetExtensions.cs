using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Common
{
    public static class LvDatasetExtensions
    {
        public static List<LvDatasetDTO> CheckRaterVersion(this IEnumerable<LvDatasetDTO> list, bool isCurrentlySupported)
        {
            List<LvDatasetDTO> result = new List<LvDatasetDTO>();

            List<string> oldVersions = new List<string> { "1", "2", "3", "4", "5", "6" };

            if (isCurrentlySupported)
            {
                // Filter for currently supported version "7"
                result = list.Where(x => x.SupportedInRaterVersion.Split(",").Any(o => o.Trim() == "7")).ToList();
            }
            else
            {
                // Filter for old versions in the "oldVersions" list
                result = list.Where(x => x.SupportedInRaterVersion.Split(",").Any(o => oldVersions.Contains(o.Trim()))).ToList();
            }

            return result;
        }
    }
}
