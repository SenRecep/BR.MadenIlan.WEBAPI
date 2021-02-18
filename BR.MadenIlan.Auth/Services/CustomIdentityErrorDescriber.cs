using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace BR.MadenIlan.Auth.Services
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email) => new IdentityError()
        {
            Code= "DuplicateEmail",
            Description=$"{email} adresi kullanılmaktadır."
        };
        public override IdentityError DuplicateUserName(string userName) => new IdentityError()
        {
            Code = "DuplicateUserName",
            Description = $"{userName} kullanıcı adı kullanılmaktadır."
        };


    }
}
