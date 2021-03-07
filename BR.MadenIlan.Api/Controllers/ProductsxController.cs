using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BR.MadenIlan.Api.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BR.MadenIlan.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsxController : ControllerBase
    {
        private readonly AppDbContext context;

        public ProductsxController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await context.Products.ToListAsync());
        }
    }
}
