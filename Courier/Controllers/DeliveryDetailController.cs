using Courier.Data;
using Courier.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Courier.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryDetailController : ControllerBase
    {
        readonly ApplicationDbContext _db;
        private readonly ILogger<DeliveryDetailController> _logger;

        public DeliveryDetailController([FromServices] ApplicationDbContext db, ILogger<DeliveryDetailController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet(Name = "GetDeliveryDetails")]
        public async Task<IEnumerable<DeliveryDetail>> GetDeliveryDetailes()
        {
            try
            {
                var deliveryDetails = await _db.DeliveryDetails.ToListAsync();
                return deliveryDetails;
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Exception in " + this.GetType().Name + "::" + System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name);
                return new List<DeliveryDetail>();
            }
        }

        [HttpPost(Name = "AddDeliveryDetail")]
        public async Task<IActionResult> AddDeliveryDetail(DeliveryDetail deliveryDetail)
        {
            _db.DeliveryDetails.Add(deliveryDetail);

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