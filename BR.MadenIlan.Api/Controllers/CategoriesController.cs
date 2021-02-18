using System.Linq;
using System.Threading.Tasks;

using BR.MadenIlan.Api.Models;

using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BR.MadenIlan.Api.Controllers
{
    [Authorize]
    public class CategoriesController :ODataController
    {
        private readonly AppDbContext context;

        public CategoriesController(AppDbContext context)
        {
            this.context = context;
        }

        [EnableQuery(PageSize = 5)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(context.Categories.AsQueryable());
        }
        [EnableQuery]
        [HttpGet]
        public IActionResult Get([FromODataUri] int key)
        {
            return Ok(context.Categories.Where(x => x.Id == key));
        }
        [EnableQuery]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Category category)
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            return Created(category);
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] Category category)
        {
            category.Id = key;
            context.Categories.Update(category);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            Category found = await context.Categories.FindAsync(key);
            if (found == null)
                return NotFound();
            context.Categories.Remove(found);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
