using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using BR.MadenIlan.Web.Server.Helpers;
using BR.MadenIlan.Web.Server.Services;
using BR.MadenIlan.Web.Shared.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BR.MadenIlan.Web.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
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
            var identityToken = await tokenService.IdentityConnectTokenAsync();
            if (identityToken is null)
                return StatusCode(StatusCodes.Status500InternalServerError, ApiHelper.ErrorHandler());
            return Ok(identityToken);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody]SignInDto signInDto)
        {
            var token = await authService.SignInAsync(signInDto);
            if (token is null)
                return StatusCode(StatusCodes.Status500InternalServerError, ApiHelper.ErrorHandler());
            return Ok(token);
        }

        [HttpGet]
        public async Task<IActionResult> CheckToken(string access_token)
        {
            var introspect = await tokenService.CheckTokenAsync(access_token);
            if (introspect is null)
                return StatusCode(StatusCodes.Status500InternalServerError,ApiHelper.ErrorHandler());
            return Ok(introspect);
        }
        [HttpGet]
        public async Task<IActionResult> RefreshToken(string refresh_token)
        {
            var token = await tokenService.RefreshTokenAsync(refresh_token);
            if (token is null)
                return StatusCode(StatusCodes.Status500InternalServerError, ApiHelper.ErrorHandler());
            return Ok(token);
        }
    }
}
