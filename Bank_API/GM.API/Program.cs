using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace GM.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureAppConfiguration((context,builder) =>
                //{
                //    builder.ad
                //})
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                              .UseUrls("http://localhost:9500");
                });
    }
}