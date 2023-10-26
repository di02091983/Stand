using Catalog.Data;
using Catalog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        readonly ApplicationDbContext _db;
        private readonly ILogger<ProductController> _logger;

        public ProductController([FromServices] ApplicationDbContext db, ILogger<ProductController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet(Name = "GetProducts")]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            try
            {
                var products = await _db.Products.ToListAsync();
                return products;
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Exception in " + this.GetType().Name + "::" + System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name);
                return new List<Product>();
            }
        }

        [HttpPost(Name = "AddProduct")]
        public async Task<IActionResult> AddProduct(Product product)
        {
            _db.Products.Add(product);

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