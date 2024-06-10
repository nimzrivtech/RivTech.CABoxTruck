using RivTech.CABoxTruck.WebService.Service.Services.Rater.ListParsers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater
{
    public interface IListValueTextParser
    {
        Task<string> ParseAsync(object listValueId, ListParserType type,int? raterVersion = null);
        Task<string> ParseLimitAsync(object listValueId, string limitType, string state); 
        Task<string> ParseLimitAsync(object listValueId, string limitType, string state, int? raterVersion = null); 
    }

    public class ListValueTextParser : IListValueTextParser
    {
        private readonly IEnumerable<IListParser> _listParsers;

        public ListValueTextParser(IEnumerable<IListParser> listParsers)
        {
            _listParsers = listParsers;
        }

        public async Task<string> ParseAsync(object listValueId, ListParserType type,int? raterVersion = null)
        {
            return await GetParserByType(type)?.ParseAsync(listValueId, raterVersion);
        }

        public async Task<string> ParseLimitAsync(object listValueId, string limitType, string state)
        {
            var limitParser = _listParsers.First(x => x is LimitsParser) as LimitsParser;
            limitParser.Type = limitType;
            limitParser.State = state;
            return await limitParser.ParseAsync(listValueId);
        }

        public async Task<string> ParseLimitAsync(object listValueId, string limitType, string state, int? raterVersion = null)
        {
            var limitParser = _listParsers.First(x => x is LimitsParser) as LimitsParser;
            limitParser.Type = limitType;
            limitParser.State = state;
            return await limitParser.ParseAsync(listValueId, raterVersion);
        }

        private IListParser GetParserByType(ListParserType type)
        {
            return type switch
            {
                ListParserType.BusinessType => _listParsers.First(p => p is BusinessTypeParser),
                ListParserType.AccountCategory => _listParsers.First(p => p is AccountCategoryParser),
                ListParserType.MainUse => _listParsers.First(p => p is MainUseParser),
                ListParserType.MainGVW => _listParsers.First(p => p is MainGVWParser),
                ListParserType.MainGVWInLbs => _listParsers.First(p => p is MainGVWInLbs),
                ListParserType.NewVenture => _listParsers.First(p => p is NewVentureParser),
                ListParserType.Symbols => _listParsers.First(p => p is SymbolsParser),
                ListParserType.VehicleDescription => _listParsers.First(p => p is VehicleDescriptionParser),
                ListParserType.RatingType => _listParsers.First(p => p is RatingTypeParser),
                ListParserType.SecondaryBusinessGroup => _listParsers.First(p => p is SecondaryBusinessGroupParser),
                _ => null,
            };
        }

    }
}