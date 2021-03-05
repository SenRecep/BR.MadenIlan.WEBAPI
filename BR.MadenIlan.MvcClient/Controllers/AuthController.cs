using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

using BR.MadenIlan.Web.Shared.Models;

using IdentityModel.Client;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace BR.MadenIlan.MvcClient.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiClient apiClient;
        private readonly HttpClient httpClient;

        public AuthController(IOptions<ApiClient> apiClientOptions, HttpClient httpClient)
        {
            apiClient = apiClientOptions.Value;
            this.httpClient = httpClient;
        }
        public IActionResult SignIn() => View();
        public async Task<IActionResult> SignIn(SignInDto signInDto)
        {
            var disco = await httpClient.GetDiscoveryDocumentAsync(apiClient.GetAuthBaseUrl);

            if (disco.IsError)
            {

            }

            var tokenResponse = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest()
            {
                Address = disco.TokenEndpoint,
                UserName = signInDto.UserName,
                Password = signInDto.Password,
                ClientId = apiClient.WebClient.ClientId,
                ClientSecret = apiClient.WebClient.ClientSecret
            });

            if (tokenResponse.IsError)
            {

            }

            var userInfo = await httpClient.GetUserInfoAsync(new UserInfoRequest()
            {
                Token = tokenResponse.AccessToken,
                Address = disco.UserInfoEndpoint
            });

            if (userInfo.IsError)
            {

            }
            var claimsPrincipal =
                new ClaimsPrincipal(
                    new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme));

            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken(){ Name=OpenIdConnectParameterNames.IdToken, Value=tokenResponse.IdentityToken},
                new AuthenticationToken(){ Name=OpenIdConnectParameterNames.AccessToken, Value=tokenResponse.AccessToken},
                new AuthenticationToken(){ Name=OpenIdConnectParameterNames.RefreshToken, Value=tokenResponse.RefreshToken},
                new AuthenticationToken(){ Name=OpenIdConnectParameterNames.ExpiresIn,
                    Value=DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)},
            });

            await HttpContext.SignInAsync(claimsPrincipal,authenticationProperties);
            return RedirectToAction("Index","User");
        }
    }
}
