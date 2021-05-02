using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BR.MadenIlan.Clients.Shared.DTOs.Auth;
using BR.MadenIlan.Clients.Shared.Services;

using Microsoft.AspNetCore.Mvc;

namespace BR.MadenIlan.Clients.BlazorServerApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService authService;
        private readonly ITokenService tokenService;

        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            this.authService = authService;
            this.tokenService = tokenService;
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await authService.SignOutAsync();
            return Redirect("/");
        }

        [HttpGet]
        public async Task<IActionResult> Login(string ReturnUrl)
        {
            await authService.SignOutAsync();
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(SignInDTO signInDTO)
        {
            if (!ModelState.IsValid)
                return View(signInDTO);

            await authService.SignOutAsync();
            var res = await authService.SignInAsync(signInDTO);

            if (!res.IsSuccessful)
            {
                ViewBag.ErrorMessage = res.GetErrors();
                return View(signInDTO);
            }
            return Redirect("/");
        }


        [HttpGet]
        public IActionResult Register() => View();



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(SignUpDTO signUpDTO)
        {
            if (!ModelState.IsValid)
                return View(signUpDTO);

            var token = await tokenService.ConnectTokenAsync();
            var res = await authService.SignUpAsync(signUpDTO, token.Success.AccessToken);

            if (!res.IsSuccessful)
            {
                ViewBag.ErrorMessage = res.GetErrors();
                return View(signUpDTO);
            }
            return RedirectToAction("Login");
        }
    }
}
