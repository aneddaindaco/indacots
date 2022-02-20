using AutoMapper;
using IndacoProject.Corso.Data.Entities;
using IndacoProject.Corso.Data.Models;
using IndacoProject.Corso.Data.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Data.Profiles
{
    public class BaseConfigValueResolver : IValueResolver<Message, MessageCsvModel, string>
    {
        protected readonly BaseConfig _options;

        public BaseConfigValueResolver(IOptions<BaseConfig> options)
        {
            _options = options.Value;
        }

        public string Resolve(Message source, MessageCsvModel destination, string destMember, ResolutionContext context)
        {
            return source.Body + (_options.Descrizione ?? "valore default");
        }
    }
}
