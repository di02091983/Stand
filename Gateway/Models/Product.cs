namespace Gateway.Models
{
    public class Product
    {
        public Guid Id { get; set; } 

        public string? Name { get; set; }

        public Guid CategoryId { get; set; }

        public Guid BrandId { get; set; }

        public int Amount { get; set; }
    }
}
