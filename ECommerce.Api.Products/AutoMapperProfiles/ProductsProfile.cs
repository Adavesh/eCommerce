using AutoMapper;

namespace ECommerce.Api.Products.AutoMapperProfiles
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            CreateMap<Db.Product, Models.Product>().ReverseMap();
        }
    }
}
