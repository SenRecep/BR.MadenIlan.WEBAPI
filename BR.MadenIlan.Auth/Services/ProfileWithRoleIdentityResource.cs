
using IdentityModel;

using IdentityServer4.Models;

namespace BR.MadenIlan.Auth.Services
{
    public class ProfileWithRoleIdentityResource: IdentityResources.Profile
    {
        public ProfileWithRoleIdentityResource()
        {
            UserClaims.Add(JwtClaimTypes.Role);
        }
    }
}
