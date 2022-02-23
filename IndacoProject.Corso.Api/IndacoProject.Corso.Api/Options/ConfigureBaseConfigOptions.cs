﻿using IndacoProject.Corso.Data.Options;
using IndacoProject.Corso.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace IndacoProject.Corso.Api.Options
{
    public class ConfigureBaseConfigOptions : IConfigureOptions<BaseConfig>
    {
        protected readonly BaseConfigService _configService;
        protected readonly BaseConfig _configuration;

        public ConfigureBaseConfigOptions(BaseConfigService configService, IConfiguration configuration)
        {
            _configService = configService;
            _configuration = configuration.GetSection("Base").Get<BaseConfig>();
        }

        public void Configure(BaseConfig options)
        {
            var newConfig = _configService.GetBaseConfig();

            options.Codice = newConfig.Codice ?? _configuration.Codice;
            options.Descrizione = newConfig.Descrizione ?? _configuration.Descrizione;

        }
    }
}
