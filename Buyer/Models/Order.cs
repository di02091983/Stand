namespace Buyer.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public string? Num { get; set; }

        public int Status { get; set;}
    }
}
