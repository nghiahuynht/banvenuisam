
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace GM.API.Validator
{
    
    public static class FluentValidationConfiguration
    {
        public static void AddFluentValidationServices(this IServiceCollection services)
        {
           
            services.AddControllers()
                .AddFluentValidation(fv =>
                    fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

        }
    }
}
