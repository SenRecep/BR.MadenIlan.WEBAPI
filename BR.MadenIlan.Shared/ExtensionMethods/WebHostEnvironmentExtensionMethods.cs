using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BR.MadenIlan.Shared.ExtensionMethods
{
    public static class WebHostEnvironmentExtensionMethods
    {
        public static string GetConnectionType(this IWebHostEnvironment environment, bool test = false)
        {
            var con = environment.IsDevelopment();
            if (test) con = !con;
            return con ? "Local" : "Server";
        }
        public static string GetIdentityServerUrl(this IWebHostEnvironment environment, IConfiguration configuration,bool test=false)
        {
            var conType = environment.GetConnectionType(test);
            var result = conType switch
            {
                "Server" => configuration.GetSection("IdentityServer:Server").Get<string>(),
                _ => configuration.GetSection("IdentityServer:Local").Get<string>()
            };
            return result;
        }
    }
}
