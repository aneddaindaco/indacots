using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Api.Extensions
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
