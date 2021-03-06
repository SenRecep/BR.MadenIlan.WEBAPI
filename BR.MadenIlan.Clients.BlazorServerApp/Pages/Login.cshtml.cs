using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BR.MadenIlan.Clients.Shared.DTOs.Auth;
using BR.MadenIlan.Clients.Shared.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BR.MadenIlan.Clients.BlazorServerApp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService authService;

        public LoginModel(IAuthService authService)
        {
            this.authService = authService;
        }
        [BindProperty]
        public SignInDTO SignInDTO { get; set; }
        [BindProperty]
        public string ErrorMessage { get; set; }
        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }
        public async Task<IActionResult> OnPost()
        {
            var res = await authService.SignInAsync(SignInDTO);

            if (!res.IsSuccessful)
            {
                ErrorMessage = res.GetErrors();
                return Page();
            }

            if (!string.IsNullOrEmpty(ReturnUrl))
                return Redirect(ReturnUrl);
            else
                return Redirect("/");

        }
    }
}
