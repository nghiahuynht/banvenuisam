using GM.BL.Service.Users;
using GM.CORE;
using GM.CORE.Enums;
using GM.DAL;
using GM.MODEL.ViewModel;
using GM.MODEL.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GM.API.Provider
{
    public class TokenProviderMiddleware
    {
        protected readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly RequestDelegate _next;
        private readonly TokenProviderOptions _options;

        public TokenProviderMiddleware(RequestDelegate next, TokenProviderOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var request = context.Request;
                var response = context.Response;

                if (!request.Path.Equals(_options.Path, StringComparison.Ordinal))
                {
                    await _next(context);
                    return;
                }

                if (!request.Method.Equals("POST") || !request.HasFormContentType)
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await response.WriteAsync(await CreateResponse("Bad Request"));
                    return;
                }

                //await GenerateToken(context);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                var response = context.Response;

                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await response.WriteAsync(await CreateResponse(ex?.Message));
            }
        }


        private async Task<string> CreateResponse(string message)
        {
            ResponseViewModel<string> response = new ResponseViewModel<string>
            {
                IsSuccess = false,
                Message = message,
                ErrorCode = StatusCodes.Status500InternalServerError
            };
            return await Task.FromResult(response.ToJsonCamel());
        }
    }
}