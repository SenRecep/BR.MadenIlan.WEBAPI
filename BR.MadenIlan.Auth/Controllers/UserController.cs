
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using BR.MadenIlan.Auth.Dtos;
using BR.MadenIlan.Auth.ExtensionMethods;
using BR.MadenIlan.Auth.Models;
using BR.MadenIlan.Shared.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using static IdentityServer4.IdentityServerConstants;

namespace BR.MadenIlan.Auth.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(LocalApi.PolicyName)]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
     
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
            };


            IdentityResult result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                ErrorDto errorDto = new ErrorDto()
                {
                    StatusCode=StatusCodes.Status400BadRequest,
                    IsShow=true,
                    Path= "api/User/SignUp"
                };
                errorDto.Errors.AddRange(result.Errors.Select(x => x.Description));
                return BadRequest(errorDto);
            }
            return NoContent();
        }
        //[HttpPost]
        //public async Task<IActionResult> GetUser()
        //{
        //    Claim userClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);

        //    if (userClaim.IsNull()) return BadRequest("Kullanici girisi dogrulanamadi");

        //    ApplicationUser user = await userManager.FindByIdAsync(userClaim.Value);

        //    if (user.IsNull()) return BadRequest("Gecerli bir kullanici bulunamadi");

        //    ApplicationUserDto dto = new ApplicationUserDto
        //    {
        //        Email = user.Email,
        //        UserName = user.UserName
        //    };

        //    return Ok(dto);
        //}
    }
}
