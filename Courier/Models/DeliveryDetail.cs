namespace Courier.Models
{
    public class DeliveryDetail
    {
        public Guid Id { get; set; }

        public Guid DeliveryId { get; set; }

        public Guid ProductId { get; set; }

        public int StatusDeliveryDetailId { get; set; }

        public int Amount { get; set; }
    }
}
