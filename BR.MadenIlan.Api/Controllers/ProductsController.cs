using System.Linq;
using System.Threading.Tasks;

using BR.MadenIlan.Api.Models;
using BR.MadenIlan.Shared.Models;

using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BR.MadenIlan.Api.Controllers
{
    [Authorize]
    public class ProductsController : ODataController
    {
        private readonly AppDbContext context;

        public ProductsController(AppDbContext context)
        {
            this.context = context;
        }
     
        [EnableQuery(PageSize = 5)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(context.Products.AsQueryable());
        }
        [EnableQuery]
        [HttpGet]
        public IActionResult Get([FromODataUri] int key)
        {
            return Ok(context.Products.Where(x => x.Id == key));
        }
        [EnableQuery]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            return Created(product);
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] Product product)
        {
            product.Id = key;
            context.Products.Update(product);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var found = await context.Products.FindAsync(key);
            if (found == null)
                return NotFound();
            context.Products.Remove(found);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
