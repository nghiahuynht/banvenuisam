using GM.MODEL.Model;
using GM.MODEL.ViewModel.User;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GM.BL.Service.Users
{
    public interface IUserService
    {
        Task<UserLogin> UserLogin(string loginName, string password);
        Task<ClaimsIdentity> GetIdentity(string loginName, string password);

    }
}