using System.Net.Http;

namespace BR.MadenIlan.Clients.Shared.Exceptions
{
    public class OfflineException: HttpRequestException
    {
        public OfflineException(string message) : base(message) { }
    }
}
