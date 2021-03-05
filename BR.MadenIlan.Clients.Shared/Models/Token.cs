
namespace BR.MadenIlan.Clients.Shared.Models
{
    public record Token(string AccessToken, int ExpiresIn, string TokenType, string Scope, string RefreshToken);

}
