using System;
using System.Security.Policy;

using BR.MadenIlan.Auth.Mapping.AutoMapperProfile;
using BR.MadenIlan.Clients.Shared.DTOs.Auth;
using BR.MadenIlan.Clients.Shared.Interceptors;
using BR.MadenIlan.Clients.Shared.Managers;
using BR.MadenIlan.Clients.Shared.Mapping.AutoMapperProfile;
using BR.MadenIlan.Clients.Shared.Models;
using BR.MadenIlan.Clients.Shared.Services;
using BR.MadenIlan.Clients.Shared.Validations.FluentValidation.Auth;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BR.MadenIlan.Clients.Shared.Containers.MicrosoftIOC
{
    public static class CustomIocExtension
    {
        public static void AddAllDependencies(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            services.AddDependencies(configuration);
            services.AddAuthDependencies();
            services.AddMapperDependencies();
            services.AddValidationDependencies();
            services.AddServicesDependencies(configuration, hostEnvironment);
            services.AddAttributeDependencies();
            services.AddHelperDependencies();
        }

        public static void AddAuthDependencies(this IServiceCollection services)
        {
            services.AddAuthentication(opts =>
            {
                opts.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opts =>
            {
                opts.LoginPath = "/Auth/Login";
                opts.AccessDeniedPath = "/Auth/AccessDenied";
            });

        }

        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiClient>(configuration.GetSection("ApiClient"));
        }
        public static void AddMapperDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(TokenProfile));
            services.AddAutoMapper(typeof(IntrospectProfile));
        }
        public static void AddValidationDependencies(this IServiceCollection services)
        {
            services.AddTransient<IValidator<SignInDTO>, SignInDtoValidator>();
            services.AddTransient<IValidator<SignUpDTO>, SignUpDtoValidator>();
        }
        public static void AddServicesDependencies(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            var apiClient = configuration.GetSection("ApiClient").Get<ApiClient>();
            apiClient.IsLocal = hostEnvironment.IsDevelopment();

            var authUrl= new Uri(apiClient.GetAuthBaseUrl);
            var apiUrl = new Uri(apiClient.GetApiBaseUrl);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddHttpClient();

            services.AddScoped<RequestLoggerInterceptor>();
            services.AddScoped<TokenInterceptor>();
            services.AddScoped<NetworkInterceptor>();

            services.AddHttpClient<IApiResourceHttpClient, ApiResourceHttpClient>(conf => conf.BaseAddress = apiUrl)
                .AddHttpMessageHandler<RequestLoggerInterceptor>()
                .AddHttpMessageHandler<NetworkInterceptor>()
                .AddHttpMessageHandler<TokenInterceptor>();

            services.AddHttpClient<ITokenService, TokenManager>(conf=> conf.BaseAddress = authUrl);
            services.AddHttpClient<IAuthService, AuthManager>(conf => conf.BaseAddress = authUrl);

            services.AddScoped<IProductService, ProductManager>();

            services.AddDistributedMemoryCache();
            //services.AddSession(options => options.IdleTimeout = TimeSpan.FromMinutes(30));

        }
        public static void AddAttributeDependencies(this IServiceCollection services)
        {

        }

        public static void AddHelperDependencies(this IServiceCollection services)
        {
            //services.AddScoped<AccountHelper>();
            //services.AddScoped<FileHelper>();
        }
        public static void AddCustomControllerServices(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddFluentValidation();
        }
    }
}
