using GM.CORE;
using GM.CORE.Enums;
using GM.MODEL.ViewModel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GM.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public abstract class ApiBaseController : ControllerBase
    {
        protected readonly ILogger logger = LogManager.GetCurrentClassLogger();

       
        protected async Task<OkObjectResult> CreateResponse<T>(T data, string messageSuccess = null)
        {
            ResponseViewModel<T> response = new ResponseViewModel<T>(data, messageSuccess);
            return await Task.FromResult(Ok(response.ToJsonCamel()));
        }

        protected async Task<OkObjectResult> CreateResponseFail(string message, int? errorCode)
        {
            ResponseViewModel<string> response = new ResponseViewModel<string>(message, errorCode);

            return await Task.FromResult(Ok(response.ToJsonCamel()));
        }

        protected async Task<OkObjectResult> ResponseSuccess(string messageSuccess = null)
        {
            ResponseModel response = new ResponseModel(messageSuccess);
            return await Task.FromResult(Ok(response.ToJsonCamel()));
        }

        protected async Task<OkObjectResult> ResponseFail(string message, int? errorCode)
        {
            ResponseModel response = new ResponseModel(message, errorCode);

            return await Task.FromResult(Ok(response.ToJsonCamel()));
        }

        protected string GetDataFromClaim(ApiClaimTypes claim)
        {
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null && identity.Claims.Any(c =>c.Type == claim.ClaimType))
            {
                return identity.Claims.FirstOrDefault(c => c.Type == claim.ClaimType).Value;
            }
            return "";
        }
       

      
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="isSuccess"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
       
        protected int? GetUserIdFromClaim()
        {
            var parsable = int.TryParse(GetDataFromClaim(ApiClaimTypes.UserId), out int userId);

            if (!parsable)
            {
                return null;
            }

            return userId;
        }
    }
}