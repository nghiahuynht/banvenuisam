using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Infrastructure.Configuration;

namespace WebApp.Infrastructure.Utilities
{
    public static partial class Helper
    {
        public static AppSettings Settings => AppSettingServices.Get;
        public static IServiceProvider ServiceProvider { get; set; }
    }
}
