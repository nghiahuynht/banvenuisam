using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GM.API.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // === GHI LOG REQUEST ===
                var request = context.Request;
                var requestBody = await ReadRequestBody(request);

                logger.Info($"[REQUEST] {request.Method} {request.Path}{request.QueryString}");
                //logger.Info($"Headers: {string.Join(" | ", request.Headers)}");
                if (!string.IsNullOrEmpty(requestBody))
                    logger.Info($"Body: {requestBody}");

                // === GHI LOG RESPONSE ===
                // Ghi response tạm vào memory stream để log lại nội dung
                var originalBodyStream = context.Response.Body;
                using var responseBody = new MemoryStream();
                context.Response.Body = responseBody;

                await _next(context); // Gọi middleware tiếp theo

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                logger.Info($"[RESPONSE] StatusCode: {context.Response.StatusCode} | Body: {responseText}");

                // Ghi response thực ra lại cho client
                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi log request/response");
                throw;
            }
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            if (request.ContentLength == null || request.ContentLength == 0)
                return string.Empty;

            request.EnableBuffering();

            request.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin);

            return body;
        }
    }
}
