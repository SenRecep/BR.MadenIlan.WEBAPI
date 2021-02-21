using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using BR.MadenIlan.Web.Shared.Models;

using IdentityModel.Client;

namespace BR.MadenIlan.WebUi.Mapping.AutoMapperProfile
{
    public class TokenProfile:Profile
    {
        public TokenProfile()
        {
            CreateMap<TokenResponse, Token>();
        }
    }
}
