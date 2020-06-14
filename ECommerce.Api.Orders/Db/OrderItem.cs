namespace ECommerce.Api.Orders.Db
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public OrderItem()
        {
        }

        public OrderItem(int id, int orderId, int productId, int qty, int unitPrice) : this()
        {
            Id = id;
            OrderId = orderId;
            ProductId = productId;
            Quantity = qty;
            UnitPrice = unitPrice;
        }
    }
}
