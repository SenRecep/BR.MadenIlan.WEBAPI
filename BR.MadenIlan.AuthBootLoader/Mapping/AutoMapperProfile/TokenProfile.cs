
using AutoMapper;

using BR.MadenIlan.Web.Shared.Models;

using IdentityModel.Client;

namespace BR.MadenIlan.AuthBootLoader.Mapping.AutoMapperProfile
{
    public class TokenProfile:Profile
    {
        public TokenProfile()
        {
            CreateMap<TokenResponse, Token>();
        }
    }
}
