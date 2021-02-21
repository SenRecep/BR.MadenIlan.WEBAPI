using System.Threading.Tasks;

using BR.MadenIlan.Web.Shared.Models;

namespace BR.MadenIlan.WebUi.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<Token>> SignInAsync(SignInDto signInDto);
        Task<ApiResponse<object>> SignUpAsync(SignUpDto signUpDto, string token);
    }
}
