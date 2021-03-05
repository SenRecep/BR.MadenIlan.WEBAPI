using System.Collections.Generic;

using BR.MadenIlan.Auth.Models;
using BR.MadenIlan.Core.StringInfo;

using Microsoft.AspNetCore.Identity;

namespace BR.MadenIlan.Auth
{
    public static class DefaultUsersAndRoles
    {
        public static IEnumerable<SignUpViewModel> GetDevelopers()
        {
            yield return new SignUpViewModel()
            {
                UserName="Daniga",
                Email="me@senrecep.com",
                Password="1943332733.Rec"
            };
            yield return new SignUpViewModel()
            {
                UserName = "Berat",
                Email = "incecamberat@gmail.com",
                Password = "123Password*"
            };
        }

        public static IEnumerable<IdentityRole> GetRoles()
        {
            yield return new IdentityRole(RoleInfo.Developer);
            yield return new IdentityRole(RoleInfo.Admin);
            yield return new IdentityRole(RoleInfo.Customer);
        }
    }
}
