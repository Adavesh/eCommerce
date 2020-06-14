using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext dbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;

        public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Orders.Any())
            {
                dbContext.Orders.AddRange(new[]
                {
                    new Db.Order(1, 1, new DateTime(2020, 1, 1, 11, 30, 30)),
                    new Db.Order(2, 1, new DateTime(2020, 1, 2, 11, 30, 33)),
                    new Db.Order(3, 2, new DateTime(2020, 2, 1, 10, 30, 27)),
                    new Db.Order(4, 3, new DateTime(2020, 2, 2, 10, 15, 12)),
                    new Db.Order(5, 4, new DateTime(2020, 2, 2, 11, 40, 25)),
                    new Db.Order(6, 4, new DateTime(2020, 3, 1, 9, 55, 45)),
                    new Db.Order(7, 4, new DateTime(2020, 3, 2, 8, 30, 58)),
                    new Db.Order(8, 5, new DateTime(2020, 3, 2, 8, 45, 24)),
                });

                dbContext.OrderItems.AddRange(new[]
                {
                    new Db.OrderItem(1, 1, 1, 2, 135),
                    new Db.OrderItem(2, 1, 2, 1, 6),
                    new Db.OrderItem(3, 2, 3, 1, 225),
                    new Db.OrderItem(4, 3, 3, 1, 225),
                    new Db.OrderItem(5, 3, 5, 1, 198),
                    new Db.OrderItem(6, 3, 4, 1, 245),
                    new Db.OrderItem(7, 4, 5, 1, 198),
                    new Db.OrderItem(8, 5, 1, 1, 135),
                    new Db.OrderItem(9, 5, 2, 1, 6),
                    new Db.OrderItem(10, 6, 4, 5, 198),
                    new Db.OrderItem(11, 7, 3, 10, 225),
                    new Db.OrderItem(12, 8, 1, 2, 135),
                    new Db.OrderItem(13, 8, 2, 2, 6),
                    new Db.OrderItem(14, 8, 3, 2, 225),
                    new Db.OrderItem(15, 8, 4, 2, 198),
                    new Db.OrderItem(16, 8, 4, 2, 245),
                });

                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                var orders = await dbContext.Orders.Include("Items").Where(o => o.CustomerId == customerId).ToListAsync();

                if (orders != null && orders.Count > 0)
                {
                    var result = mapper.Map<IEnumerable<Models.Order>>(orders);
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
    }
}
