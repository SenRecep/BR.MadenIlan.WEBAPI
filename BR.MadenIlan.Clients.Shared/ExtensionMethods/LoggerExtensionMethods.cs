using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BR.MadenIlan.Clients.Shared.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;

namespace BR.MadenIlan.Clients.Shared.ExtensionMethods
{
    public static class LoggerExtensionMethods
    {
        public static void SerilogInit()
        {
            var logTemplate = "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}";

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .WriteTo.Console(new RenderedCompactJsonFormatter())
                .WriteTo.Debug(outputTemplate: DateTime.Now.ToString())
                .WriteTo.Console(outputTemplate: logTemplate, theme: AnsiConsoleTheme.Code)
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day,outputTemplate: logTemplate)
                .CreateLogger();
        }

        public static void LogApiResponse<T,R>(this ILogger<T> logger,ApiResponse<R> apiResponse) where R:class
        {
            if (apiResponse.IsSuccessful && apiResponse.Success is SuccessMessageResponse res)
                logger.LogInformation(res.Message);
            if (!apiResponse.IsSuccessful)
            {
                var errors = apiResponse.GetErrors("/n");
                if (apiResponse.Fail.StatusCode == StatusCodes.Status500InternalServerError)
                    logger.LogError(errors);
                else
                    logger.LogWarning(errors);
            }
        }
    }
}
