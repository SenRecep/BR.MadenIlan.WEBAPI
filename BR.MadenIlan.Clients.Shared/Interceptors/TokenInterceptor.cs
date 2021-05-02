using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using BR.MadenIlan.Clients.Shared.Services;
using BR.MadenIlan.Core.ExtensionMethods;

using IdentityModel.Client;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace BR.MadenIlan.Clients.Shared.Interceptors
{
    public class TokenInterceptor : DelegatingHandler
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ITokenService tokenService;
        private readonly ILogger<TokenInterceptor> logger;

        public TokenInterceptor(IHttpContextAccessor httpContextAccessor, ITokenService tokenService, ILogger<TokenInterceptor> logger)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.tokenService = tokenService;
            this.logger = logger;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            request.SetBearerToken(accessToken);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refreshToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

                if (!refreshToken.IsEmpty())
                {
                    var tokenResponse = await tokenService.RefreshTokenSignInAsync(refreshToken);
                    if (tokenResponse.IsSuccessful)
                        response = await SendAsync(request, cancellationToken);
                    else
                        response.Content = new StringContent("/auth/login");
                }
            }

            return response;
        }
    }
}
