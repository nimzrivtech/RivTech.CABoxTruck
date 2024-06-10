using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater
{
    public interface IListParser
    {
        Task<string> ParseAsync(object id,int? raterVersion = null);
    }
}
