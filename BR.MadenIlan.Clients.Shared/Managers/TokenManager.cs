using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using BR.MadenIlan.Clients.Shared.ExtensionMethods;
using BR.MadenIlan.Clients.Shared.Helpers;
using BR.MadenIlan.Clients.Shared.Models;
using BR.MadenIlan.Clients.Shared.Services;

using IdentityModel.Client;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace BR.MadenIlan.Clients.Shared.Managers
{
    public class TokenManager : ITokenService
    {
        private readonly HttpClient client;
        private readonly ApiClient apiClient;
        private readonly IMapper mapper;
        private readonly ILogger<TokenManager> logger;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TokenManager(HttpClient client, IOptions<ApiClient> apiClientOptions, IMapper mapper, ILogger<TokenManager> logger,IHttpContextAccessor httpContextAccessor)
        {
            this.client = client;
            this.mapper = mapper;
            this.logger = logger;
            this.httpContextAccessor = httpContextAccessor;
            apiClient = apiClientOptions.Value;
            client.BaseAddress = new Uri(apiClient.GetAuthBaseUrl);
        }

        public async Task<ApiResponse<Token>> ConnectTokenAsync()
        {
            TokenResponse res = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = "connect/token",
                ClientId = apiClient.IdentityClient.ClientId,
                ClientSecret = apiClient.IdentityClient.ClientSecret,
                Scope = "IdentityServerApi"
            });

            Token result = mapper.Map<Token>(res);
            var apiResponse = await ApiHelper.Result(res, result, "TokenManager/ConnectTokenAsync");
            logger.LogApiResponse(apiResponse);
            return apiResponse;
        }

        public async Task<ApiResponse<Introspect>> CheckTokenAsync(string token)
        {
            client.SetBasicAuthentication(apiClient.BasicUserName, apiClient.BasicPassword);

            TokenIntrospectionResponse res = await client.IntrospectTokenAsync(new TokenIntrospectionRequest()
            {
                Address = "connect/introspect",
                ClientId = apiClient.WebClient.ClientId,
                ClientSecret = apiClient.WebClient.ClientSecret,
                Token = token
            });

            Introspect result = mapper.Map<Introspect>(res);
            var apiResponse = await ApiHelper.Result(res, result, "TokenManager/CheckTokenAsync");
            logger.LogApiResponse(apiResponse);
            return apiResponse;

        }

        public async Task<ApiResponse<Token>> RefreshTokenAsync(string refreshToken)
        {
            TokenResponse res = await client.RequestRefreshTokenAsync(new RefreshTokenRequest()
            {
                Address = "connect/token",
                ClientId = apiClient.WebClient.ClientId,
                ClientSecret = apiClient.WebClient.ClientSecret,
                RefreshToken = refreshToken
            });
            Token token = mapper.Map<Token>(res);

            var apiResponse = await ApiHelper.Result(res, token, "TokenManager/RefreshTokenAsync");
            logger.LogApiResponse(apiResponse);
            return apiResponse;
        }

        public async Task<ApiResponse<SuccessMessageResponse>> RefreshTokenSignInAsync(string refreshToken)
        {
            var tokenResponse = await RefreshTokenAsync(refreshToken);
            if (!tokenResponse.IsSuccessful)
                return new(false, null, tokenResponse.Fail);

            var token = tokenResponse.Success;

            List<AuthenticationToken> tokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken{ Name=OpenIdConnectParameterNames.AccessToken,Value= token.AccessToken},
                new AuthenticationToken{ Name=OpenIdConnectParameterNames.RefreshToken,Value= token.RefreshToken},
                new AuthenticationToken{ Name=OpenIdConnectParameterNames.ExpiresIn,
                    Value= DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o", CultureInfo.InvariantCulture)}
            };

            AuthenticateResult authenticationResult = await httpContextAccessor.HttpContext.AuthenticateAsync();

            AuthenticationProperties properties = authenticationResult.Properties;

            properties.StoreTokens(tokens);

            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal, properties);
            ApiResponse<SuccessMessageResponse> result = new(true, new("Kullanici refresh token kayit islemi tamamlandi"));
            logger.LogApiResponse(result);
            return result;
        }
    }
}
