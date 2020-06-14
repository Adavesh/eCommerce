using System;
using System.Collections.Generic;

namespace ECommerce.Api.Orders.Db
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> Items { get; set; }

        public decimal Total
        {
            get
            {
                var total = 0M;

                if (Items != null && Items.Count > 0)
                {
                    foreach (var item in Items)
                    {
                        total += item.Quantity * item.UnitPrice;
                    }
                }

                return total;
            }
        }


        public Order()
        {

        }

        public Order(int id, int customerId, DateTime orderDateTime) : this()
        {
            Id = id;
            CustomerId = customerId;
            OrderDate = orderDateTime;
        }
    }
}
