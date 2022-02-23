using IndacoProject.Corso.Data.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Services
{
    public class BaseConfigService
    {
        protected readonly IServiceProvider _serviceProvider;

        public BaseConfigService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public BaseConfig GetBaseConfig()
        {
            return new BaseConfig() { Codice = "fromService", Descrizione = "Servizio from Indaco" };
        }
    }
}
