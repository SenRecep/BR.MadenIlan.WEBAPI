using System;
using System.Net.Http;
using System.Threading.Tasks;

using AutoMapper;

using BR.MadenIlan.Auth.Helpers;
using BR.MadenIlan.Auth.Services;
using BR.MadenIlan.Web.Shared.Models;

using IdentityModel.Client;

using Microsoft.Extensions.Options;


namespace BR.MadenIlan.Auth.Managers
{
    public class TokenManager : ITokenService
    {
        private readonly HttpClient client;
        private readonly ApiClient apiClient;
        private readonly IMapper mapper;

        public TokenManager(HttpClient client, IOptions<ApiClient> apiClientOptions, IMapper mapper)
        {
            this.client = client;
            this.mapper = mapper;
            apiClient = apiClientOptions.Value;
        }

        public async Task<ApiResponse<Token>> ConnectTokenAsync()
        {
            client.BaseAddress = new Uri(apiClient.AuthBaseUrl);

            TokenResponse res = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = "connect/token",
                ClientId = apiClient.IdentityClient.ClientId,
                ClientSecret = apiClient.IdentityClient.ClientSecret,
                Scope = "IdentityServerApi"
            });

            Token result = mapper.Map<Token>(res);

            return ApiHelper.Result(res, result, "TokenManager/ConnectTokenAsync");

        }

        public async Task<ApiResponse<Introspect>> CheckTokenAsync(string token)
        {
            client.BaseAddress = new Uri(apiClient.AuthBaseUrl);
            client.SetBasicAuthentication(apiClient.BasicUserName, apiClient.BasicPassword);

            TokenIntrospectionResponse res = await client.IntrospectTokenAsync(new TokenIntrospectionRequest()
            {
                Address = "connect/introspect",
                ClientId = apiClient.WebClient.ClientId,
                ClientSecret = apiClient.WebClient.ClientSecret,
                Token = token
            });

            Introspect result = mapper.Map<Introspect>(res);

            return ApiHelper.Result(res, result, "TokenManager/CheckTokenAsync");

        }

        public async Task<ApiResponse<Token>> RefreshTokenAsync(string refreshToken)
        {
            client.BaseAddress = new Uri(apiClient.AuthBaseUrl);

            TokenResponse res = await client.RequestRefreshTokenAsync(new RefreshTokenRequest()
            {
                Address = "connect/token",
                ClientId = apiClient.WebClient.ClientId,
                ClientSecret = apiClient.WebClient.ClientSecret,
                RefreshToken = refreshToken
            });


            Token result = mapper.Map<Token>(res);

            return ApiHelper.Result(res, result, "TokenManager/RefreshTokenAsync");
        }
    }
}
