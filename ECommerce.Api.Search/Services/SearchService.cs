using System.Linq;
using System.Threading.Tasks;
using ECommerce.Api.Search.Interfaces;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrderService orderService;
        private readonly IProductsService productsService;
        private readonly ICustomerService customerService;

        public SearchService(IOrderService orderService, IProductsService productsService, ICustomerService customerService)
        {
            this.orderService = orderService;
            this.productsService = productsService;
            this.customerService = customerService;
        }

        public async Task<(bool IsSuccess, dynamic SearchResult)> SearchAsync(int customerId)
        {
            var (IsSuccess, Orders, ErrorMessage) = await orderService.GetOrdersAsync(customerId);

            if (IsSuccess)
            {
                var products = await productsService.GetProductsAsync();
                var customers = await customerService.GetCustomersAsync();

                foreach (var order in Orders)
                {
                    foreach (var item in order.Items)
                    {
                        item.ProductName = products.IsSuccess ? products.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name : "Product name not available";
                    }
                }

                var customerName = customers.IsSuccess ? customers.Customers.FirstOrDefault(c => c.Id == customerId)?.Name : "Customer info not avialable";

                return (true, new { Customer = customerName, Orders });
            }

            return (false, null);
        }
    }
}
