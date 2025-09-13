using GM.CORE;
using GM.CORE.AppExceptions;
using GM.MODEL.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace GM.API.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        protected readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                logger.Error(exception.Message);

                var response = context.Response;
                response.ContentType = "application/json";

                var responseModel = new ResponseViewModel<string>()
                {
                    IsSuccess = false,
                    Message = exception?.ToString()
                };

                switch (exception)
                {
                    case ApiException ex:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                await response.WriteAsync(responseModel.ToJsonCamel());
            }
        }
    }
}