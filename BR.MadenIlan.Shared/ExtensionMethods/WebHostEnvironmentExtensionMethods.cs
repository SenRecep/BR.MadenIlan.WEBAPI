
using System;

using BR.MadenIlan.Shared.Models;
using BR.MadenIlan.Shared.ExtensionMethods;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;


namespace BR.MadenIlan.Shared.ExtensionMethods
{
    public static class WebHostEnvironmentExtensionMethods
    {
        public static ConnectionType GetConnectionType(this IWebHostEnvironment environment, bool test = false)
        {
            bool con = environment.IsDevelopment();
            if (test) con = !con;
            return con ? ConnectionType.Local: ConnectionType.Server;
        }


        public static string GetCustomConnectionString(this IConfiguration configuration, ConnectionType type)
        {
            var mechineName = Environment.MachineName;
            var connectionTypeName = Enum.GetName(typeof(ConnectionType), type);
            var result = type switch
            {
                ConnectionType.Server => configuration.GetConnectionString(connectionTypeName),
                ConnectionType.Local => configuration.GetSection($"LocalConnectionStrings:{mechineName}").Get<string>(),
                _ => throw new NotImplementedException(),
            };
            if (type == ConnectionType.Local && result.IsEmpty())
                result = configuration.GetSection($"LocalConnectionStrings:Default").Get<string>();
            return result;
        }


        public static string GetIdentityServerUrl(this IWebHostEnvironment environment, IConfiguration configuration,bool test=false)
        {
            ConnectionType conType = environment.GetConnectionType(test);
            var connectionTypeName = Enum.GetName(typeof(ConnectionType), conType);
            string result = conType switch
            {
                ConnectionType.Server => configuration.GetSection($"IdentityServer:{connectionTypeName}").Get<string>(),
                ConnectionType.Local => configuration.GetSection($"IdentityServer:{connectionTypeName}").Get<string>(),
                _ => throw new NotImplementedException()
            };
            return result;
        }
    }
}
