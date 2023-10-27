namespace Buyer.Models
{
    public class OrderDetail
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public int Amount { get; set; }
    }
}
