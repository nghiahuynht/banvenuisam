using AutoMapper;
using GM.MODEL;
using Microsoft.Extensions.DependencyInjection;

namespace GM.API
{
    public static class StartupProfileMapper
    {
        public static IServiceCollection AddProfileMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfiler));
            return services;
        }
    }
}