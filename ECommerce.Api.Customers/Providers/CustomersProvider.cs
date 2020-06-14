using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomersDbContext dbContext;
        private readonly ILogger<CustomersProvider> logger;
        private readonly IMapper mapper;

        public CustomersProvider(CustomersDbContext dbContext, ILogger<CustomersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Customers.Any())
            {
                dbContext.Customers.AddRange(new[]
                {
                    new Db.Customer(1, "Adavesh Managaon", "Koramangala"),
                    new Db.Customer(2, "Virat Kohli", "New Delhi"),
                    new Db.Customer(3, "Dean Winchester", "Kansas"),
                    new Db.Customer(4, "Hannah Baker", "New York"),
                    new Db.Customer(5, "Lucifer Morningstar", "Hell"),
                });

                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, Models.Customer Customer, string ErrorMessage)> GetCustomerAsync(int customerId)
        {
            try
            {
                var customer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
                if (customer != null)
                {
                    var result = mapper.Map<Models.Customer>(customer);
                    return (true, result, null);
                }

                return (false, null, "Not Found");
            }
            catch (Exception e)
            {
                logger?.LogError(e.Message, e);
                return (false, null, e.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Customer> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var customers = await dbContext.Customers.ToListAsync();
                if (customers != null && customers.Count > 0)
                {
                    var results = mapper.Map<IEnumerable<Models.Customer>>(customers);
                    return (true, results, null);
                }

                return (false, null, "Not Found");
            }
            catch (Exception e)
            {
                logger?.LogError(e.Message, e);
                return (false, null, e.Message);
            }
        }
    }
}
