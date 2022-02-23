using IndacoProject.Corso.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Services.Extensions
{
    public static class IOC
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IEmailBackgroundService, EmailBackgroundService>();
            services.AddSingleton<ICipherService, CipherService>();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddHostedService(o => o.GetService<IEmailBackgroundService>());
            services.AddSingleton<BaseConfigService>();
            return services;
        }
    }
}
