using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using BR.MadenIlan.Web.Shared.Models;
using BR.MadenIlan.WebUi.Services;

using IdentityModel.Client;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BR.MadenIlan.WebUi.Managers
{
    public class TokenManager : ITokenService
    {
        private readonly HttpClient client;
        private readonly IMapper mapper;
        private readonly ApiClient apiClient;
        public TokenManager(HttpClient client, IOptions<ApiClient> apiClientOptions,IMapper mapper)
        {
            this.client = client;
            this.mapper = mapper;
            apiClient = apiClientOptions.Value;
        }
        public async Task<ApiResponse<Token>> ConnectTokenAsync()
        {
            client.BaseAddress = new Uri(apiClient.AuthBaseUrl);
            Dictionary<string, string> dict = new Dictionary<string, string>
            {
                { "client_id", apiClient.IdentityClient.ClientId },
                { "client_secret", apiClient.IdentityClient.ClientSecret },
                { "grant_type", apiClient.ClientCredentialGrantType }
            };

            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, "connect/token") { Content = new FormUrlEncodedContent(dict) };
            string error = string.Empty;
            HttpResponseMessage res = null;
            try
            {
                res = await client.SendAsync(req);
            }
            catch (HttpRequestException ex) { error = ex.Message; }
            catch (Exception ex) { error = ex.Message; }

            if (!string.IsNullOrEmpty(error))
                return new(false, null, new(false, StatusCodes.Status500InternalServerError, "TokenManager/IdentityConnectTokenAsync", error));

            if (res.IsSuccessStatusCode)
                return new(true, await res.Content.ReadFromJsonAsync<Token>());

            string jsonResult = await res.Content.ReadAsStringAsync();
            JObject result = JsonConvert.DeserializeObject<JObject>(jsonResult);

            return new(false, null, new(false, StatusCodes.Status400BadRequest, "TokenManager/IdentityConnectTokenAsync", result.GetValue("error").Value<string>()));
        }

        public async Task<ApiResponse<Introspect>> CheckTokenAsync(string token)
        {
            client.BaseAddress = new Uri(apiClient.AuthBaseUrl);
            client.SetBasicAuthentication(apiClient.BasicUserName, apiClient.BasicPassword);

            var res = await client.IntrospectTokenAsync(new()
            {
                Address = "connect/introspect",
                ClientId = apiClient.WebClient.ClientId,
                ClientSecret = apiClient.WebClient.ClientSecret,
                Token = token
            });

            if (res.IsError)
                return new(false, null, new(false, StatusCodes.Status500InternalServerError, "TokenManager/CheckTokenAsync", res.Error));


            return new(true, mapper.Map<Introspect>(res));
        }

        public async Task<ApiResponse<Token>> RefreshTokenAsync(string refreshToken)
        {
            client.BaseAddress = new Uri(apiClient.AuthBaseUrl);

            var res = await client.RequestRefreshTokenAsync(new()
            {
                Address = "connect/token",
                ClientId = apiClient.WebClient.ClientId,
                ClientSecret = apiClient.WebClient.ClientSecret,
                RefreshToken = refreshToken
            });

            if (res.IsError)
                return new(false, null, new(false, StatusCodes.Status500InternalServerError, "TokenManager/RefreshTokenAsync", res.Error));

            return new(true, mapper.Map<Token>(res));
        }
    }
}
