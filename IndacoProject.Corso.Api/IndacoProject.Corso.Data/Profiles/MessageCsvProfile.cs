using AutoMapper;
using IndacoProject.Corso.Data.Entities;
using IndacoProject.Corso.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Data.Profiles
{
    public class MessageCsvProfile : Profile
    {
        public MessageCsvProfile()
        {
            CreateMap<Message, MessageCsvModel>()
                .ForMember(
                    dest=>dest.Body, 
                    opt=>opt.MapFrom<BaseConfigValueResolver>());

        }
    }
}
