using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;

namespace ECommerce.Api.Search.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ProductsService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<(bool IsSuccess, IEnumerable<Product> Products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient("ProductsService");
                var result = await httpClient.GetAsync("api/products");

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsByteArrayAsync();
                    var products = JsonSerializer.Deserialize<IEnumerable<Product>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    return (true, products, null);
                }

                return (false, null, result.ReasonPhrase);
            }
            catch (Exception e)
            {
                return (false, null, e.Message);
            }
        }
    }
}
