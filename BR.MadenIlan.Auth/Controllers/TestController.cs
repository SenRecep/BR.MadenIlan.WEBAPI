using System.Collections.Generic;
using System.Threading.Tasks;

using BR.MadenIlan.Auth.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BR.MadenIlan.Auth.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        public TestController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<IActionResult>Test()
        {
            var users = await userManager.Users.ToListAsync();
            return Ok(users);
        }
    }
}
