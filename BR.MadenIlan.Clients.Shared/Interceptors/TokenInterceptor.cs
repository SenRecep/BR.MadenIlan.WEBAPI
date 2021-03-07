using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using IdentityModel.Client;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace BR.MadenIlan.Clients.Shared.Interceptors
{
    public class TokenInterceptor : DelegatingHandler
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public TokenInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            request.SetBearerToken(accessToken);

            var response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}
