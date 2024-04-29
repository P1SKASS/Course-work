namespace Shop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PostOffice { get; set; }
        public string Status { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public Order() => OrderItems = new List<OrderItem>();
    }
}
