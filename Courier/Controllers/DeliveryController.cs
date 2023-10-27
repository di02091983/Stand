using Courier.Data;
using Courier.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Courier.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryController : ControllerBase
    {
        readonly ApplicationDbContext _db;
        private readonly ILogger<DeliveryController> _logger;

        public DeliveryController([FromServices] ApplicationDbContext db, ILogger<DeliveryController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet(Name = "GetDeliveries")]
        public async Task<IEnumerable<Delivery>> GetDeliveries()
        {
            try
            {
                var deliveries = await _db.Deliveries.ToListAsync();
                return deliveries;
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Exception in " + this.GetType().Name + "::" + System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name);
                return new List<Delivery>();
            }
        }

        [HttpPost(Name = "AddDelivery")]
        public async Task<IActionResult> AddDelivery(Delivery delivery)
        {
            _db.Deliveries.Add(delivery);

            try
            {
                await _db.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Exception in " + this.GetType().Name + "::" + System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}