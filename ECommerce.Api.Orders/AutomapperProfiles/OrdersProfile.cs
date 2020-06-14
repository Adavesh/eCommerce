using AutoMapper;

namespace ECommerce.Api.Orders.AutomapperProfiles
{
    public class OrdersProfile : Profile
    {
        public OrdersProfile()
        {
            CreateMap<Db.OrderItem, Models.OrderItem>().ReverseMap();
            CreateMap<Db.Order, Models.Order>();
        }
    }
}
