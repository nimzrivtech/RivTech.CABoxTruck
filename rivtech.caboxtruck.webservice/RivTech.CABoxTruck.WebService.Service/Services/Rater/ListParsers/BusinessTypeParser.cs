using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater.ListParsers
{
    public class BusinessTypeParser : IListParser
    {
        private readonly IBusinessTypeService _businessTypeService;
        public BusinessTypeParser(IBusinessTypeService businessTypeService)
        {
            _businessTypeService = businessTypeService;
        }

        public async Task<string> ParseAsync(object id, int? raterVersion = null)
        {
            var businessType = await _businessTypeService.GetBusinessTypeAsync((short)id);
            return businessType.Description;
        }
    }
}
