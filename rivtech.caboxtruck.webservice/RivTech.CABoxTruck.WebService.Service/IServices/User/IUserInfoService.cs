using RivTech.CABoxTruck.WebService.DTO.User;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.User
{
    public interface IUserInfoService
    {
        Task<string> LoginAsync(LoginDto model);
        Task<string> LogoutAsync(LoginDto model);
        Task<string> LockAccountAsync(LoginDto model);
        Task<string> LockedAccountAsync(LoginDto model);
        Task<string> FailedLoginAsync(LoginDto model);
    }
}
