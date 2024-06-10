using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IApplogService
    {
        Task Save(string message, string jsonMessage);
        Task LogDocumentGeneration(string message, string jsonMessage, string response);
    }
}
