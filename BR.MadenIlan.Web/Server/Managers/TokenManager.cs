using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using BR.MadenIlan.Web.Server.Services;
using BR.MadenIlan.Web.Shared.Models;

using Microsoft.Extensions.Options;

namespace BR.MadenIlan.Web.Server.Managers
{
    public class TokenManager : ITokenService
    {
        private readonly HttpClient client;
        private readonly ApiClient apiClient;
        public TokenManager(HttpClient client, IOptions<ApiClient> apiClientOptions)
        {
            this.client = client;
            apiClient = apiClientOptions.Value;
        }
        public async Task<Token> IdentityConnectTokenAsync()
        {
            client.BaseAddress = new Uri(apiClient.AuthBaseUrl);
            var dict = new Dictionary<string, string>
            {
                { "client_id", apiClient.IdentityClient.ClientId },
                { "client_secret", apiClient.IdentityClient.ClientSecret },
                { "grant_type", apiClient.ClientCredentialGrantType }
            };

            var req = new HttpRequestMessage(HttpMethod.Post, "connect/token") { Content = new FormUrlEncodedContent(dict) };
            var res = await client.SendAsync(req);

            if (res.IsSuccessStatusCode)
               return await res.Content.ReadFromJsonAsync<Token>();

            return null;
        }

        public async Task<Introspect> CheckTokenAsync(string token)
        {
            client.BaseAddress = new Uri(apiClient.AuthBaseUrl);
            Dictionary<string, string> dict = new Dictionary<string, string>
            {
                { "token", token }
            };

            client.DefaultRequestHeaders.Authorization= new AuthenticationHeaderValue(
        "Basic", Convert.ToBase64String(
            Encoding.ASCII.GetBytes(
               $"{apiClient.BasicUserName}:{apiClient.BasicPassword}")));

            var req = new HttpRequestMessage(HttpMethod.Post, "connect/introspect") { Content = new FormUrlEncodedContent(dict) };
            var res = await client.SendAsync(req);

            if (res.IsSuccessStatusCode)
                return await res.Content.ReadFromJsonAsync<Introspect>();

            return null;
        }

        public async Task<Token> RefreshTokenAsync(string refreshToken)
        {
            client.BaseAddress = new Uri(apiClient.AuthBaseUrl);
            var dict = new Dictionary<string, string>
            {
                { "client_id", apiClient.WebClient.ClientId },
                { "client_secret", apiClient.WebClient.ClientSecret },
                { "grant_type", apiClient.RefreshTokenCredentialGrantType },
                { "refresh_token",  refreshToken}
            };

            var req = new HttpRequestMessage(HttpMethod.Post, "connect/token") { Content = new FormUrlEncodedContent(dict) };
            var res = await client.SendAsync(req);

            if (res.IsSuccessStatusCode)
                return await res.Content.ReadFromJsonAsync<Token>();

            return null;
        }

        
    }
}
