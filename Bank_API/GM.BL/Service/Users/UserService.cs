using AutoMapper;
using GM.CORE;
using GM.DAL;
using GM.MODEL.Model;
using GM.MODEL.ViewModel.User;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GM.BL.Service.Users
{
    internal class UserService : IUserService
    {
        private readonly IDalService _dalService;
        private readonly IMapper _mapper;

        public UserService(IDalService dalService, IMapper mapper)
        {
            _dalService = dalService;
            _mapper = mapper;
        }

        public async Task<UserLogin> UserLogin(string loginName, string password)
        {
            using (var context = _dalService.Connection())
            {
                return await context.UnitOfWork.UserRepository.UserLogin(loginName, password);
            }
        }

        public async Task<ClaimsIdentity> GetIdentity(string loginName, string password)
        {
            using (var context = _dalService.Connection())
            {
                var user = await context.UnitOfWork.UserRepository.UserLogin(loginName, password);

                if (user == null)
                {
                    return null;
                }

                // Set Claim User
                IList<Claim> claims = new List<Claim>
                {
                    new Claim(ApiClaimTypes.UserId.ClaimType, user.Id.ToString()),
                    new Claim(ApiClaimTypes.UserName.ClaimType, user.UserName),
                };

                return new ClaimsIdentity(claims);
            }
           
        }


    }
}