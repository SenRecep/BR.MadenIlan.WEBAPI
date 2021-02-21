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

using Newtonsoft.Json;

namespace BR.MadenIlan.Web.Server.Managers
{
    public class AuthManager : IAuthService
    {
        private readonly HttpClient client;
        private readonly ApiClient apiClient;
        public AuthManager(HttpClient client, IOptions<ApiClient> apiClientOptions)
        {
            this.client = client;
            apiClient = apiClientOptions.Value;
        }
        public async Task<Token> SignInAsync(SignInDto signInDto)
        {
            client.BaseAddress = new Uri(apiClient.AuthBaseUrl);
            var dict = new Dictionary<string, string>
            {
                { "client_id", apiClient.WebClient.ClientId },
                { "client_secret", apiClient.WebClient.ClientSecret },
                { "grant_type", apiClient.ResourceOwnerPasswordCredentialGrantType },
                { "username", signInDto.UserName },
                { "password", signInDto.Password }
            };

            var req = new HttpRequestMessage(HttpMethod.Post, "connect/token") { Content = new FormUrlEncodedContent(dict) };
            var res = await client.SendAsync(req);

            if (res.IsSuccessStatusCode)
                return await res.Content.ReadFromJsonAsync<Token>();

            return null;
        }

        public async Task<bool> SignUpAsync(SignUpDto signUpDto, string token)
        {
            client.BaseAddress = new Uri(apiClient.AuthBaseUrl);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = await Task.Run(() => JsonConvert.SerializeObject(signUpDto));

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await client.PostAsync("api/user/SignUp", httpContent);

            return res.IsSuccessStatusCode;
        }

     
    }
}
