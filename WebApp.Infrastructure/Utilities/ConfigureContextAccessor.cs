using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Infrastructure.Utilities
{
    public static partial class ConfigureContext
    {
        private static IHttpContextAccessor _httpContextAccessor;
        private static HttpContext _context => _httpContextAccessor.HttpContext;
        public static void ConfigureContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public static string GetRequestPath()
        {
            if (_context != null)
            {
                if (!string.IsNullOrEmpty(_context.Request.Path.Value))
                {
                    return _context.Request.Path.Value;
                }
            }
            return string.Empty;
        }
    }
}
