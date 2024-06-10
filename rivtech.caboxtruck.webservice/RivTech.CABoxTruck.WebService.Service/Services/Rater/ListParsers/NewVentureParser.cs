using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater.ListParsers
{
    public class NewVentureParser : IListParser
    {
        public Task<string> ParseAsync(object id, int? raterVersion = null)
        {
            if (id is null) return Task.FromResult("");

            string text = (short) id switch
            {
                1 => "New Venture - No Driving Experience",
                2 => "New Venture - Owned Another Company",
                3 => "New Venture - Owner Currently Owns Another Company",
                4 => "New Venture - Owner Was Driver For Another Company",
                _ => ""
            };
            return Task.FromResult(text);
        }
    }
}
