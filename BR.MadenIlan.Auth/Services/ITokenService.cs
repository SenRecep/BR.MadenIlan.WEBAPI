using System.Threading.Tasks;

using BR.MadenIlan.Web.Shared.Models;

namespace BR.MadenIlan.Auth.Services
{
    public interface ITokenService
    {
        Task<ApiResponse<Token>> ConnectTokenAsync();
        Task<ApiResponse<Introspect>> CheckTokenAsync(string token);
        Task<ApiResponse<Token>> RefreshTokenAsync(string refreshToken);
    }
}
