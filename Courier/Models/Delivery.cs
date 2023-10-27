namespace Courier.Models
{
    public class Delivery
    {
        public Guid Id { get; set; } 

        public string? Num { get; set; }

        public int StatusDeliveryId { get; set; }
    }
}
