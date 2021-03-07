using System.Net.Http;

using BR.MadenIlan.Clients.Shared.Services;

namespace BR.MadenIlan.Clients.Shared.Managers
{
    public class ApiResourceHttpClient : IApiResourceHttpClient
    {
        private readonly HttpClient httpClient;

        public ApiResourceHttpClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public HttpClient GetHttpClient()
        {
            return httpClient;
        }
    }
}
