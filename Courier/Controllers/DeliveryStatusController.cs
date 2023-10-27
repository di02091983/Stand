using Courier.Data;
using Courier.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Courier.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryStatusController : ControllerBase
    {
        readonly ApplicationDbContext _db;
        private readonly ILogger<DeliveryStatusController> _logger;

        public DeliveryStatusController([FromServices] ApplicationDbContext db, ILogger<DeliveryStatusController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet(Name = "GetDeliveryStatuses")]
        public async Task<IEnumerable<DeliveryStatus>> GetDeliveryStatuses()
        {
            try
            {
                var deliveryStatuses = await _db.DeliveryStatuses.ToListAsync();
                return deliveryStatuses;
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Exception in " + this.GetType().Name + "::" + System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name);
                return new List<DeliveryStatus>();
            }
        }

        [HttpPost(Name = "AddDeliveryStatus")]
        public async Task<IActionResult> AddDeliveryStatus(DeliveryStatus deliveryStatus)
        {
            _db.DeliveryStatuses.Add(deliveryStatus);

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