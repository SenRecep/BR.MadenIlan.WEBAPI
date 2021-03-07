using System.Net.Http;

namespace BR.MadenIlan.Clients.Shared.Services
{
    public interface IApiResourceHttpClient
    {
        HttpClient GetHttpClient();
    }
}
