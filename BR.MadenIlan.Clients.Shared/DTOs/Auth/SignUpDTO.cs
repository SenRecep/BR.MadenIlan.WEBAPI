
using BR.MadenIlan.Clients.Shared.DTOs.Interfaces;

namespace BR.MadenIlan.Clients.Shared.DTOs.Auth
{
    public record SignUpDTO(string UserName, string Email, string Password) :IDTO;
}
