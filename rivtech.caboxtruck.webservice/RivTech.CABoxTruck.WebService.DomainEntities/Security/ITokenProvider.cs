using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Security
{
    public interface ITokenProvider
    {
        Task<string> GetAdminTokenAsync(); 
    }
}
