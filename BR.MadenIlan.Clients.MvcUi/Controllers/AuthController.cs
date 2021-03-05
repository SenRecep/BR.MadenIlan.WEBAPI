using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BR.MadenIlan.Clients.Shared.DTOs.Auth;
using BR.MadenIlan.Clients.Shared.Services;

using Microsoft.AspNetCore.Mvc;

namespace BR.MadenIlan.Clients.MvcUi.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await authService.SignOutAsync();
            return RedirectToAction("Login", "Auth");
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

            var res = await authService.SignInAsync(signInDTO);

            if (!res.IsSuccessful)
            {
                ViewBag.ErrorMessage = res.GetErrors();
                return View(signInDTO);
            }
            return RedirectToAction("Index","User");
        }


    }
}
