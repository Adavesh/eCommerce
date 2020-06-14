using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using Microsoft.Extensions.Logging;

namespace ECommerce.Api.Search.Services
{
    public class CustomersService : ICustomerService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<CustomersService> logger;

        public CustomersService(IHttpClientFactory httpClientFactory, ILogger<CustomersService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public async Task<(bool IsSuccess, IEnumerable<Customer> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient("CustomersService");
                var result = await httpClient.GetAsync("api/customers");

                if (result.IsSuccessStatusCode)
                {

                    var textResponse = await result.Content.ReadAsStringAsync();

                    //var content = await result.Content.ReadAsByteArrayAsync();



                    var customers = JsonSerializer.Deserialize<IEnumerable<Customer>>(textResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    return (true, customers, null);
                }

                return (false, null, result.ReasonPhrase);
            }
            catch (Exception e)
            {
                logger?.LogError(e.Message);
                return (false, null, e.Message);
            }
        }
    }
}
