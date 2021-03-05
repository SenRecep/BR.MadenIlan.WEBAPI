using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using BR.MadenIlan.Auth.Models;

using IdentityModel;

using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

using Microsoft.AspNetCore.Identity;

namespace BR.MadenIlan.Auth.Services
{
    public class CustomProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public CustomProfileService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subId = context.Subject.GetSubjectId();
            var user = await userManager.FindByIdAsync(subId);
            var userRoles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>()
            {
               new Claim(JwtRegisteredClaimNames.Email, user.Email),
               new Claim( "name", user.UserName),
            };

            foreach (var role in userRoles)
                claims.Add(new Claim("role",role));

            context.AddRequestedClaims(claims);

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subId = context.Subject.GetSubjectId();
            var user = await userManager.FindByIdAsync(subId);
            context.IsActive = user != null;
        }
    }
}

