using System.Threading.Tasks;

using BR.MadenIlan.Web.Shared.Models;

namespace BR.MadenIlan.Web.Server.Services
{
    public interface ITokenService
    {
        Task<Token> IdentityConnectTokenAsync();
        Task<Introspect> CheckTokenAsync(string token);
        Task<Token> RefreshTokenAsync(string refreshToken);
    }
}
