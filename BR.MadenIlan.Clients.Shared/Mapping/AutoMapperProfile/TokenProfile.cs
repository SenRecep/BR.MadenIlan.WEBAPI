
using AutoMapper;

using BR.MadenIlan.Clients.Shared.Models;

using IdentityModel.Client;

namespace BR.MadenIlan.Auth.Mapping.AutoMapperProfile
{
    public class TokenProfile:Profile
    {
        public TokenProfile()
        {
            CreateMap<TokenResponse, Token>();
        }
    }
}
