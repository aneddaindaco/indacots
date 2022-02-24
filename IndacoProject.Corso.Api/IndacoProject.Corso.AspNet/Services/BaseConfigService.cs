using IndacoProject.Corso.Data.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndacoProject.Corso.AspNet.Services
{
<<<<<<< HEAD
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
=======
    ////public class BaseConfigService
    ////{
    ////    protected readonly IServiceProvider _serviceProvider;

    ////    public BaseConfigService(IServiceProvider serviceProvider)
    ////    {
    ////        _serviceProvider = serviceProvider;
    ////    }

    ////    public BaseConfig GetBaseConfig()
    ////    {
    ////        return new BaseConfig() { Codice = "fromService", Descrizione = "Servizio from Indaco" };
    ////    }
    ////}
>>>>>>> 2fa3e60847f54f6ab2a8cbe56864faba6ed3b07a
}
