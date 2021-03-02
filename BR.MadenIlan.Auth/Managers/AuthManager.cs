using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using BR.MadenIlan.Auth.Helpers;
using BR.MadenIlan.Auth.Services;
using BR.MadenIlan.Web.Shared.Models;

using IdentityModel.Client;

using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace BR.MadenIlan.Auth.Managers
{
    public class AuthManager : IAuthService
    {
        private readonly HttpClient client;
        private readonly ApiClient apiClient;
        private readonly IMapper mapper;
        public AuthManager(HttpClient client, IOptions<ApiClient> apiClientOptions,IMapper mapper)
        {
            this.client = client;
            this.mapper = mapper;
            apiClient = apiClientOptions.Value;
        }

        public async Task<ApiResponse<Token>> SignInAsync(SignInDto signInDto)
        {
            client.BaseAddress = new Uri(apiClient.AuthBaseUrl);
            TokenResponse res = await client.RequestPasswordTokenAsync(new PasswordTokenRequest() {
                Address = "connect/token",
                ClientId = apiClient.WebClient.ClientId,
                ClientSecret = apiClient.WebClient.ClientSecret,
                UserName=signInDto.UserName,
                Password=signInDto.Password
            });

            Token result = mapper.Map<Token>(res);

            return ApiHelper.Result(res, result, "AuthManager/SignInAsync");
        }

        public async Task<ApiResponse<SuccessMessageResponse>> SignUpAsync(SignUpDto signUpDto, string token)
        {
            client.BaseAddress = new Uri(apiClient.AuthBaseUrl);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string json = await Task.Run(() => JsonConvert.SerializeObject(signUpDto));

            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage res = await client.PostAsync("api/user/SignUp", httpContent);

            if (res.IsSuccessStatusCode)
                return new ApiResponse<SuccessMessageResponse>(true,new SuccessMessageResponse("Kullanici kayit islemi tamamlandi"));
            var resultContent = await res.Content.ReadAsStringAsync();
            ErrorDto errorDto = JsonConvert.DeserializeObject<ErrorDto>(resultContent);
            return new ApiResponse<SuccessMessageResponse>(false, null,errorDto);
        }
    }
}
