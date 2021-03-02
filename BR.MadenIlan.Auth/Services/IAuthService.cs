﻿using System.Threading.Tasks;

using BR.MadenIlan.Web.Shared.Models;

namespace BR.MadenIlan.Auth.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<Token>> SignInAsync(SignInDto signInDto);
        Task<ApiResponse<SuccessMessageResponse>> SignUpAsync(SignUpDto signUpDto, string token);
    }
}
