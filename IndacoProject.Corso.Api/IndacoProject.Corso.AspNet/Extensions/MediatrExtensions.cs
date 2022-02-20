using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Extensions
{
    public static class MediatrExtensions
    {
        public static IServiceCollection ServiceMediatr(this IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain
               .GetAssemblies()
               .Where(o => o.GetName().Name.StartsWith("IndacoProject.Corso"))
               .ToArray();
            services.AddMediatR(assemblies);

            return services;
        }
    }
}
