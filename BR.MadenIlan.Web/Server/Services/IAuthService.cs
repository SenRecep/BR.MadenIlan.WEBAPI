using System.Threading.Tasks;

using BR.MadenIlan.Web.Shared.Models;

namespace BR.MadenIlan.Web.Server.Services
{
    public interface IAuthService
    {
        Task<Token> SignInAsync(SignInDto signInDto);
        Task<bool> SignUpAsync(SignUpDto signUpDto, string token);
    }
}
