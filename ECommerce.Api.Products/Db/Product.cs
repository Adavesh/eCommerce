namespace ECommerce.Api.Products.Db
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Inventory { get; set; }

        public Product()
        {

        }

        public Product(int id, string name, decimal price, int qty)
        {
            Id = id;
            Name = name;
            Price = price;
            Inventory = qty;
        }
    }
}
