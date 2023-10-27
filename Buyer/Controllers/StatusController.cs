using Buyer.Data;
using Buyer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Buyer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        readonly ApplicationDbContext _db;
        private readonly ILogger<StatusController> _logger;

        public StatusController([FromServices] ApplicationDbContext db, ILogger<StatusController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet(Name = "GetStatuses")]
        public async Task<IEnumerable<Status>> GetStatuses()
        {
            try
            {
                var statuses = await _db.Statuses.ToListAsync();
                return statuses;
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Exception in " + this.GetType().Name + "::" + System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name);
                return new List<Status>();
            }
        }

        [HttpPost(Name = "AddStatus")]
        public async Task<IActionResult> AddStatus(Status status)
        {
            _db.Statuses.Add(status);

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