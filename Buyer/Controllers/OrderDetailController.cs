using Buyer.Data;
using Buyer.Models;
using Buyer.RabbitMq;
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
        private readonly IRabbitMqService _mqService;

        public OrderDetailController([FromServices] ApplicationDbContext db, ILogger<OrderDetailController> logger, IRabbitMqService mqService)
        {
            _db = db;
            _logger = logger;
            _mqService = mqService;
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
        public async Task<IActionResult> AddOrderDetail(OrderDetail orderDetail)
        {
            _db.OrderDetails.Add(orderDetail);

            try
            {
                _mqService.SendMessage(orderDetail);

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