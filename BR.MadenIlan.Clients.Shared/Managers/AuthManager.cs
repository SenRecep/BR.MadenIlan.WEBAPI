using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

using AutoMapper;

using BR.MadenIlan.Clients.Shared.DTOs.Auth;
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

using Newtonsoft.Json;

namespace BR.MadenIlan.Clients.Shared.Managers
{
    public class AuthManager : IAuthService
    {
        private readonly HttpClient client;
        private readonly ApiClient apiClient;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<AuthManager> logger;

        public AuthManager(
            HttpClient client,
            IOptions<ApiClient> apiClientOptions,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AuthManager> logger)
        {
            this.client = client;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
            apiClient = apiClientOptions.Value;
            client.BaseAddress = new Uri(apiClient.GetAuthBaseUrl);
        }

        public async Task<ApiResponse<IEnumerable<Claim>>> GetUserInfoAsync(string access_token)
        {
            UserInfoResponse userinfo = await client.GetUserInfoAsync(new()
            {
                Address = "connect/userinfo",
                Token = access_token
            });
            var apiResponse = await ApiHelper.Result(userinfo, userinfo.Claims, "AuthManager/GetUserInfoAsync");
            logger.LogApiResponse(apiResponse);
            return apiResponse;
        }
        public async Task<ApiResponse<Token>> RequestPasswordTokenAsync(SignInDTO signInDto)
        {
            TokenResponse tokenResponse = await client.RequestPasswordTokenAsync(new()
            {
                Address = "connect/token",
                ClientId = apiClient.WebClient.ClientId,
                ClientSecret = apiClient.WebClient.ClientSecret,
                UserName = signInDto.UserName,
                Password = signInDto.Password
            });

            Token result = mapper.Map<Token>(tokenResponse);

            var apiResponse = await ApiHelper.Result(tokenResponse, result, "AuthManager/RequestPasswordTokenAsync");
            logger.LogApiResponse(apiResponse);
            return apiResponse;
        }
        public async Task<ApiResponse<SuccessMessageResponse>> SignInAsync(SignInDTO signInDto)
        {
            ApiResponse<Token> tokenResponse = await RequestPasswordTokenAsync(signInDto);
            if (!tokenResponse.IsSuccessful)
                return new(false, null, tokenResponse.Fail);
            Token token = tokenResponse.Success;

            ApiResponse<IEnumerable<Claim>> userInfoResponse = await GetUserInfoAsync(token.AccessToken);
            if (!userInfoResponse.IsSuccessful)
                return new(false, null, tokenResponse.Fail);
            IEnumerable<Claim> userInfos = userInfoResponse.Success;

            ClaimsPrincipal claimsPrincipal = new(new ClaimsIdentity(
                userInfos,
                CookieAuthenticationDefaults.AuthenticationScheme,
                "name",
                "role"
               ));

            AuthenticationProperties authenticationProperties = new();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new(){ Name=OpenIdConnectParameterNames.AccessToken,Value= token.AccessToken},
                new(){ Name=OpenIdConnectParameterNames.RefreshToken,Value= token.RefreshToken},
                new(){ Name=OpenIdConnectParameterNames.ExpiresIn,
                    Value= DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o", CultureInfo.InvariantCulture)}
            });


            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

            ApiResponse<SuccessMessageResponse> result = new(true, new("Kullanici Giris islemi basari ile gerceklesti"));
            logger.LogApiResponse(result);
            return result;
        }
        public async Task<ApiResponse<SuccessMessageResponse>> SignUpAsync(SignUpDTO signUpDto, string access_token)
        {
            ApiResponse<SuccessMessageResponse> result;

            client.SetBearerToken(access_token);
            HttpResponseMessage res = await client.PostAsJsonAsync("api/user/SignUp", signUpDto);

            if (!res.IsSuccessStatusCode)
            {
                string resultContent = await res.Content.ReadAsStringAsync();
                ErrorDto errorDto = JsonConvert.DeserializeObject<ErrorDto>(resultContent);
                result = new(false, null, errorDto);
                logger.LogApiResponse(result);
                return result;
            }
            result = new(true, new("Kullanici kayit islemi tamamlandi"));
            logger.LogApiResponse(result);
            return result;
        }
        public async Task<ApiResponse<SuccessMessageResponse>> SignOutAsync()
        {
            await httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ApiResponse<SuccessMessageResponse> res = new(true, new("Kullanici çıkış işlemi gerçekleşti"));
            logger.LogApiResponse(res);
            return res;
        }
    }
}