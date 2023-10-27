using Catalog.Data;
using Catalog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : ControllerBase
    {
        readonly ApplicationDbContext _db;
        private readonly ILogger<BrandController> _logger;

        public BrandController([FromServices] ApplicationDbContext db, ILogger<BrandController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet(Name = "GetBrands")]
        public async Task<IEnumerable<Brand>> GetBrands()
        {
            try
            {
                var brands = await _db.Brands.ToListAsync();
                return brands;
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Exception in " + this.GetType().Name + "::" + System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name);
                return new List<Brand>();
            }
        }

        [HttpPost(Name = "AddBrand")]
        public async Task<IActionResult> AddBrand(Brand brand)
        {
            _db.Brands.Add(brand);

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