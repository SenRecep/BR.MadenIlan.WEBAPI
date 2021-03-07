using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using BR.MadenIlan.Clients.Shared.Exceptions;

namespace BR.MadenIlan.Clients.Shared.Interceptors
{
    //System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
    public class NetworkInterceptor : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                throw new OfflineException("Internet Bağlantısı Yok");
            return base.SendAsync(request, cancellationToken);
        }
    }
}
