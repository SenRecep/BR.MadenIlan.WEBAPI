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

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        public async Task<IActionResult> Logout()
        {
            await authService.SignOutAsync();
            return Redirect("/");
        }

        public IActionResult Login(string ReturnUrl)
        {
            var usr = User;
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [HttpPost]
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
    }
}
