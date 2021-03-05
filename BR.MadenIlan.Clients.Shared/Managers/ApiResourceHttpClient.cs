using System.Net.Http;
using System.Threading.Tasks;

using BR.MadenIlan.Clients.Shared.Services;

using IdentityModel.Client;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace BR.MadenIlan.Clients.Shared.Managers
{
    public class ApiResourceHttpClient : IApiResourceHttpClient
    {
        private readonly HttpClient httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiResourceHttpClient(HttpClient httpClient,IHttpContextAccessor httpContextAccessor )
        {
            this.httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HttpClient> GetHttpClient()
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            httpClient.SetBearerToken(accessToken);

            return httpClient;
        }
    }
}
