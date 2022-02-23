using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Api.Extensions
{
    public static class AutoMapperExtensions
    {
        public static IServiceCollection ServiceAutoMapper(this IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain
               .GetAssemblies()
               .Where(o => o.GetName().Name.StartsWith("IndacoProject.Corso"))
               .ToArray();
            services.AddAutoMapper(assemblies);

            return services;
        }
    }
}
