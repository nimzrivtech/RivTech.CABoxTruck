using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.DataSets;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater.ListParsers
{
    public class MainUseParser : IListParser
    {
        private readonly IMainUseRepository _mainUseRepository;
        public MainUseParser(IMainUseRepository mainUseRepository)
        {
            _mainUseRepository = mainUseRepository;
        }

        public async Task<string> ParseAsync(object id, int? raterVersion = null)
        {
            var mainUse = await _mainUseRepository.FindAsync(x => x.Id == (short)id);
            return mainUse.Description;
        }
    }
}
