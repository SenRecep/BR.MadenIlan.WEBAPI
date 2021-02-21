using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using BR.MadenIlan.Web.Shared.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using BR.MadenIlan.WebUi.Services;

namespace BR.MadenIlan.WebUi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : APIBaseController
    {
        private readonly ITokenService tokenService;
        private readonly IAuthService authService;

        public AuthController(ITokenService tokenService,IAuthService authService)
        {
            this.tokenService = tokenService;
            this.authService = authService;
        }
        // /api/Auth/IdentityConnectToken
        [HttpGet]
        public async Task<IActionResult> IdentityConnectToken()
        {
            var response = await tokenService.ConnectTokenAsync();
            return ApiResponseAction(response);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody]SignInDto signInDto)
        {
            var response = await authService.SignInAsync(signInDto);
            return ApiResponseAction(response);
        }

        [HttpGet]
        public async Task<IActionResult> CheckToken(string access_token)
        {
            var response = await tokenService.CheckTokenAsync(access_token);
            return ApiResponseAction(response);
        }
        [HttpGet]
        public async Task<IActionResult> RefreshToken(string refresh_token)
        {
            var response = await tokenService.RefreshTokenAsync(refresh_token);
            return ApiResponseAction(response);
        }
    }
}
