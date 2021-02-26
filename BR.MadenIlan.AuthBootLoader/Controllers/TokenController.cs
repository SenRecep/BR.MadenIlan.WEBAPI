using System.Threading.Tasks;

using BR.MadenIlan.AuthBootLoader.Services;
using BR.MadenIlan.Web.Shared.Models;

using Microsoft.AspNetCore.Mvc;


namespace BR.MadenIlan.AuthBootLoader.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController:APIBaseController
    {
        private readonly ITokenService tokenService;

        public TokenController(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }
        [HttpGet]
        public async Task<IActionResult> ConnectToken()
        {
            ApiResponse<Token> response = await tokenService.ConnectTokenAsync();
            return ApiResponseAction(response);
        }
        [HttpGet]
        public async Task<IActionResult> CheckToken(string access_token)
        {
            ApiResponse<Introspect> response = await tokenService.CheckTokenAsync(access_token);
            return ApiResponseAction(response);
        }
        [HttpGet]
        public async Task<IActionResult> RefreshToken(string refresh_token)
        {
            ApiResponse<Token> response = await tokenService.RefreshTokenAsync(refresh_token);
            return ApiResponseAction(response);
        }
    }
}
