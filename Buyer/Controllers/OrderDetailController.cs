using Buyer.Data;
using Buyer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Buyer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailController : ControllerBase
    {
        readonly ApplicationDbContext _db;
        private readonly ILogger<OrderDetailController> _logger;

        public OrderDetailController([FromServices] ApplicationDbContext db, ILogger<OrderDetailController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet(Name = "GetOrderDetails")]
        public async Task<IEnumerable<OrderDetail>> GetOrderDetails()
        {
            try
            {
                var orderDetails = await _db.OrderDetails.ToListAsync();
                return orderDetails;
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Exception in " + this.GetType().Name + "::" + System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name);
                return new List<OrderDetail>();
            }
        }

        [HttpPost(Name = "AddOrderDetail")]
        public async Task<IActionResult> AddOrderDetail(OrderDetail OrderDetail)
        {
            _db.OrderDetails.Add(OrderDetail);

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