using System;


using BR.MadenIlan.Auth.Mapping.AutoMapperProfile;
using BR.MadenIlan.Clients.Shared.DTOs.Auth;
using BR.MadenIlan.Clients.Shared.Managers;
using BR.MadenIlan.Clients.Shared.Mapping.AutoMapperProfile;
using BR.MadenIlan.Clients.Shared.Models;
using BR.MadenIlan.Clients.Shared.Services;
using BR.MadenIlan.Clients.Shared.Validations.FluentValidation.Auth;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BR.MadenIlan.Clients.Shared.Containers.MicrosoftIOC
{
    public static class CustomIocExtension
    {
        public static void AddAllDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDependencies(configuration);
            services.AddAuthDependencies();
            services.AddMapperDependencies();
            services.AddValidationDependencies();
            services.AddServicesDependencies();
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
            services.AddTransient<IValidator<SignInDTO>, SignInDtoValidation>();
        }
        public static void AddServicesDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddHttpClient();

            services.AddHttpClient<IApiResourceHttpClient, ApiResourceHttpClient>();
            services.AddHttpClient<ITokenService, TokenManager>();
            services.AddHttpClient<IAuthService, AuthManager>();

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
