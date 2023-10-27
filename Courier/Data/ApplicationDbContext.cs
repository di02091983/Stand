using Courier.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Courier.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Delivery> Deliveries { get; set; }

        public DbSet<DeliveryDetail> DeliveryDetails { get; set; }

        public DbSet<DeliveryStatus> DeliveryStatuses { get; set; }

        public DbSet<DeliveryDetailStatus> DeliveryDetailStatuses { get; set; }
    }
}
