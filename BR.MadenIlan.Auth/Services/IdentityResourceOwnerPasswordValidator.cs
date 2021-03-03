using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            static Dictionary<string, object> errors()
            {
                return new Dictionary<string, object>
                {
                    { "errors", new List<string> { "Kullanıcı adı veya parolanız hatalı" } },
                    { "statusCode", StatusCodes.Status400BadRequest },
                    { "isShow", true },
                    { "path", "api/user/signin" }
                }; 
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
