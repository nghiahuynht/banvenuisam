using GM.API.Provider;
using GM.BL.Service.Users;
using GM.CORE;
using GM.MODEL.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Threading.Tasks;

namespace GM.API.Controllers
{
    [ApiController]
    public class UserController : ApiBaseController
    {
        private readonly IUserService _userService;

        private readonly TokenProviderOptions _options;
        protected readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public UserController(IUserService userService, TokenProviderOptions options
            )
        {
            _userService = userService;
            _options = options;
        }


        #region Login

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
                {
                    return await CreateResponseFail("UserName and Password are required.", StatusCodes.Status400BadRequest);
                }


                string loginName = model.UserName;
                string password = model.Password;

                var identity = await _userService.GetIdentity(loginName, password);

                if (identity == null)
                {
                    return await CreateResponseFail("Invalid UserName or Password", StatusCodes.Status401Unauthorized);

                }


                DateTime currentDay = DateTime.Now;
                DateTime expiredDay = currentDay.Add(_options.Expiration);

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = identity,
                    NotBefore = currentDay,
                    Expires = expiredDay,
                    SigningCredentials = _options.SigningCredentials
                };

                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

                LoginViewModel result = new LoginViewModel()
                {
                    AccessToken = tokenHandler.WriteToken(token),
                    TokenType = "Bearer",
                    StartedTime = currentDay.ToString("yyyy/MM/dd hh:mm"),
                    ExpiredTime = expiredDay.ToString("yyyy/MM/dd hh:mm"),
                };

                return await CreateResponse<LoginViewModel>(result);

                //return Ok(result); // Trả về kết quả thành công
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, "An error occurred while processing the login request.");
                return await CreateResponseFail(ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        #endregion


        //[HttpGet("GetAccountInfo")]
        //public async Task<IActionResult> GetAccountInfo()
        //{
        //    try
        //    {
        //        var userId = Convert.ToInt32(GetDataFromClaim(ApiClaimTypes.UserId));
        //        //var result = await _userService.GetUserById(userId);
        //        return await CreateResponse<int>(1);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex);
        //        return await CreateResponseFail(ex.Message, StatusCodes.Status400BadRequest);
        //        throw;

        //    }
        //}


        //[HttpPost]
        //[Route("GetAccountManagementInfo")]
        //public async Task<IActionResult> GetAccountManagementInfo(UserIdentityViewModel model)
        //{
        //    try
        //    {
        //        var result = await _userService.GetAccountManagementInfo(model);

        //        return await CreateResponse<AccountManagement>(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message);
        //        throw;
        //    }
        //}


    }


}