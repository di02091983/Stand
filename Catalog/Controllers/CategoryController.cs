using Catalog.Data;
using Catalog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        readonly ApplicationDbContext _db;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController([FromServices] ApplicationDbContext db, ILogger<CategoryController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet(Name = "GetCategories")]
        public async Task<IEnumerable<Category>> GetCategories()
        {
            try
            {
                var categories = await _db.Categories.ToListAsync();

                return categories;
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Exception in " + this.GetType().Name + "::" + System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name);
                return new List<Category>();
            }
        }

        [HttpPost(Name = "AddCategory")]
        public async Task<IActionResult> AddCategory(Category category)
        {
            _db.Categories.Add(category);

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