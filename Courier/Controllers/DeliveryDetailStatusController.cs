using Courier.Data;
using Courier.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Courier.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryDetailStatusController : ControllerBase
    {
        readonly ApplicationDbContext _db;
        private readonly ILogger<DeliveryDetailStatusController> _logger;

        public DeliveryDetailStatusController([FromServices] ApplicationDbContext db, ILogger<DeliveryDetailStatusController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet(Name = "GetDeliveryDetailStatuses")]
        public async Task<IEnumerable<DeliveryDetailStatus>> GetDeliveryDetailStatuses()
        {
            try
            {
                var deliveryDetailStatuses = await _db.DeliveryDetailStatuses.ToListAsync();
                return deliveryDetailStatuses;
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Exception in " + this.GetType().Name + "::" + System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name);
                return new List<DeliveryDetailStatus>();
            }
        }

        [HttpPost(Name = "AddDeliveryDetailStatus")]
        public async Task<IActionResult> AddDeliveryDetailStatus(DeliveryDetailStatus deliveryDetailStatus)
        {
            _db.DeliveryDetailStatuses.Add(deliveryDetailStatus);

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