using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BR.MadenIlan.Auth.ExtensionMethods;
using BR.MadenIlan.Auth.Models;

using IdentityModel;

using IdentityServer4.Validation;

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
            var existUser = await userManager.FindByNameAsync(context.UserName);
            if (existUser.IsNull()) return;
            var passwordCheck = await userManager.CheckPasswordAsync(existUser, context.Password);
            if (passwordCheck.IsNull()) return;
            context.Result = new GrantValidationResult(existUser.Id, OidcConstants.AuthenticationMethods.Password);
        }
    }
}
