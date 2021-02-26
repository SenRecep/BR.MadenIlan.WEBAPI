using System.Threading.Tasks;

using BR.MadenIlan.AuthBootLoader.Services;
using BR.MadenIlan.Web.Shared.Models;

using Microsoft.AspNetCore.Mvc;

namespace BR.MadenIlan.AuthBootLoader.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : APIBaseController
    {
        private readonly ITokenService tokenService;
        private readonly IAuthService authService;

        public AuthController(ITokenService tokenService, IAuthService authService)
        {
            this.tokenService = tokenService;
            this.authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInDto signInDto)
        {
            var response = await authService.SignInAsync(signInDto);
            return ApiResponseAction(response);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto signUpDto)
        {
            var connectToken = await tokenService.ConnectTokenAsync();
            var response = await authService.SignUpAsync(signUpDto, connectToken?.Success?.AccessToken);
            return ApiResponseAction(response);
        }

    }
}
