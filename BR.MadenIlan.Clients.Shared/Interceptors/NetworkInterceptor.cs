using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BR.MadenIlan.Clients.Shared.Interceptors
{
    //System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
    public class NetworkInterceptor:DelegatingHandler
    {
        private int _count = 0;
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            System.Threading.Interlocked.Increment(ref _count);
            request.Headers.Add("X-Custom-Header", _count.ToString());
            return base.SendAsync(request, cancellationToken);
        }
    }
}
