using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater.ListParsers
{
    public class SymbolsParser : IListParser
    {
        public Task<string> ParseAsync(object id, int? raterVersion = null)
        {
            string text = (short) id switch
            {
                1 => "7, 8 & 9",
                2 => "7 & 9",
                3 => "2, 8 & 9",
                4 => "1) Any Auto",
                5 => "2) All Owned Autos",
                6 => "3) Owned Private Passenger Autos",
                7 => "4) Owned Autos Other Than Private Passenger",
                8 => "5) All Owned Autos Which Require No-Fault Coverage",
                9 => "6) Owned Autos Subject to Compulsory UM Law",
                10 => "7) Autos Specified on  Schedule",
                11 => "8) Hired Autos",
                12 => "9) Non-Owned Autos",
                13 => "7 & 8",
                14 => "10 - Use 2, 8 & 9 Pricing",
                15 => "10 - Use 7, 8 & 9 Pricing",
                _ => ""
            };
            return Task.FromResult(text);
        }
    }
}
