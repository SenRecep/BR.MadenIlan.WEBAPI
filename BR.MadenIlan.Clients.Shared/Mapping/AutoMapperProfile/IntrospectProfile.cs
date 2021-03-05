
using AutoMapper;

using BR.MadenIlan.Clients.Shared.Models;

using IdentityModel.Client;

namespace BR.MadenIlan.Clients.Shared.Mapping.AutoMapperProfile
{
    public class IntrospectProfile : Profile
    {
        public IntrospectProfile()
        {
            CreateMap<TokenIntrospectionResponse, Introspect>();
        }
    }
}
