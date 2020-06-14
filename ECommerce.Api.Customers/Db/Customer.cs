namespace ECommerce.Api.Customers.Db
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public Customer()
        {

        }

        public Customer(int id, string name, string address) : this()
        {
            Id = id;
            Name = name;
            Address = address;
        }
    }
}
