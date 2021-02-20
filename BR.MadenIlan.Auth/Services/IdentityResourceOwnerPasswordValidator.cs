using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BR.MadenIlan.Auth.ExtensionMethods;
using BR.MadenIlan.Auth.Models;

using IdentityModel;

using IdentityServer4.Validation;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BR.MadenIlan.Auth.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> userManager;

        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            Dictionary<string, object> errors()
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string> { "Kullanıcı adı veya parolanız hatalı" });
                errors.Add("status", StatusCodes.Status400BadRequest);
                errors.Add("isShow", true);
                return errors;
            }
           
            var existUser = await userManager.FindByNameAsync(context.UserName);
            if (existUser is null)
            {
                context.Result.CustomResponse = errors();
                return;
            }
            var passwordCheck = await userManager.CheckPasswordAsync(existUser, context.Password);
            if (passwordCheck is false)
            {
                context.Result.CustomResponse = errors();
                return;
            }

            context.Result = new GrantValidationResult(existUser.Id, OidcConstants.AuthenticationMethods.Password);
        }
    }
}
