
using BR.MadenIlan.Clients.Shared.DTOs.Interfaces;

namespace BR.MadenIlan.Clients.Shared.DTOs.Auth
{
    public record SignInDTO(string UserName, string Password) : IDTO;
}
