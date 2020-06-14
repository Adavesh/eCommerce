using AutoMapper;

namespace ECommerce.Api.Customers.AutomapperProfiles
{
    public class CustomersProfile : Profile
    {
        public CustomersProfile()
        {
            CreateMap<Db.Customer, Models.Customer>().ReverseMap();
        }
    }
}
