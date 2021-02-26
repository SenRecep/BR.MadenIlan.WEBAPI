using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using BR.MadenIlan.AuthBootLoader.Helpers;
using BR.MadenIlan.AuthBootLoader.Services;
using BR.MadenIlan.Web.Shared.Models;

using IdentityModel.Client;

using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace BR.MadenIlan.AuthBootLoader.Managers
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
            TokenResponse res = await client.RequestPasswordTokenAsync(new() {
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

            HttpResponseMessage res = await client.PostAsJsonAsync("api/user/SignUp", signUpDto);

            if (res.IsSuccessStatusCode)
                return new(true,new("Kullanici kayit islemi tamamlandi"));
            var resultContent = await res.Content.ReadAsStringAsync();
            ErrorDto errorDto = JsonConvert.DeserializeObject<ErrorDto>(resultContent);
            return new(false, null,errorDto);
        }
    }
}
