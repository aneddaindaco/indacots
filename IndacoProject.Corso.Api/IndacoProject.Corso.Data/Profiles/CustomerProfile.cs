using AutoMapper;
using IndacoProject.Corso.Data.Entities.Northwind;
using IndacoProject.Corso.Data.Models;

namespace IndacoProject.Corso.Data.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerModel>().ReverseMap();
        }
    }
}
