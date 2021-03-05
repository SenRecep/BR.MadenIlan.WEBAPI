using System.Net.Http;
using System.Threading.Tasks;

namespace BR.MadenIlan.Clients.Shared.Services
{
    public interface IApiResourceHttpClient
    {
        Task<HttpClient> GetHttpClient();
    }
}
