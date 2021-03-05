using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using BR.MadenIlan.Clients.Shared.DTOs.Auth;
using BR.MadenIlan.Clients.Shared.Models;

namespace BR.MadenIlan.Clients.Shared.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<IEnumerable<Claim>>> GetUserInfoAsync(string access_token);
        Task<ApiResponse<Token>> RequestPasswordTokenAsync(SignInDTO signInDto);
        Task<ApiResponse<SuccessMessageResponse>> SignInAsync(SignInDTO signInDto);
        Task<ApiResponse<SuccessMessageResponse>> SignUpAsync(SignUpDTO signUpDto, string access_token);
        Task<ApiResponse<SuccessMessageResponse>> SignOutAsync();
    }
}
