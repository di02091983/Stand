using Buyer.Data;
using Buyer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Buyer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        readonly ApplicationDbContext _db;
        private readonly ILogger<OrderController> _logger;

        public OrderController([FromServices] ApplicationDbContext db, ILogger<OrderController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet(Name = "GetOrders")]
        public async Task<IEnumerable<Order>> GetOrders()
        {
            try
            {
                var orders = await _db.Orders.ToListAsync();
                return orders;
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Exception in " + this.GetType().Name + "::" + System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name);
                return new List<Order>();
            }
        }

        [HttpPost(Name = "AddOrder")]
        public async Task<IActionResult> AddOrder(Order order)
        {
            _db.Orders.Add(order);

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

        [HttpPut(Name = "CancelOrder")]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            try
            {
                var order = await _db.Orders.Where(x => x.Id == orderId).FirstOrDefaultAsync();

                if (order == null)
                {
                    return BadRequest();
                }

                order.Status = 2;
                _db.Update(order);

                await _db.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Exception in " + this.GetType().Name + "::" + System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost(Name = "TakeOrder")]
        public async Task<IActionResult> TakeOrder(Guid orderId)
        {
            try
            {
                var order = await _db.Orders.Where(x => x.Id == orderId).FirstOrDefaultAsync();

                if (order == null)
                {
                    return BadRequest();
                }

                order.Status = 3;
                _db.Update(order);

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