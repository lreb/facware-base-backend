using AutoMapper;
using Facware.Domain.Entities;
using Facware.Infrastructure.ViewModel;

namespace Facware.Infrastructure.Mapping
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            // CreateMap<CustomerModel, Customer>()
            //     .ForMember(dest => dest.Id,
            //             opt => opt.MapFrom(src => src.CustomerId))
            //     .ReverseMap();
        }
    }
}