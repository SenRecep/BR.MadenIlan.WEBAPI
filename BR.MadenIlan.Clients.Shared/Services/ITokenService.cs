using System.Threading.Tasks;

using BR.MadenIlan.Clients.Shared.Models;

namespace BR.MadenIlan.Clients.Shared.Services
{
    public interface ITokenService
    {
        Task<ApiResponse<Token>> ConnectTokenAsync();
        Task<ApiResponse<Introspect>> CheckTokenAsync(string token);
        Task<ApiResponse<Token>> RefreshTokenAsync(string refreshToken);
    }
}
