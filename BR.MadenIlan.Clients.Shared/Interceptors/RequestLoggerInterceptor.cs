using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace BR.MadenIlan.Clients.Shared.Interceptors
{
    public class RequestLoggerInterceptor : DelegatingHandler
    {
        private readonly ILogger<RequestLoggerInterceptor> logger;

        public RequestLoggerInterceptor(ILogger<RequestLoggerInterceptor> logger)
        {
            this.logger = logger;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"{request.RequestUri} request begin");
            var response = await base.SendAsync(request, cancellationToken);
            logger.LogInformation($"{request.RequestUri} request end");
            return response;
        }
    }
}
