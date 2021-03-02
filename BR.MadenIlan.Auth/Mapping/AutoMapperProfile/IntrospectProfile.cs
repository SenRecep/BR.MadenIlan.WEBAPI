
using AutoMapper;

using BR.MadenIlan.Web.Shared.Models;

using IdentityModel.Client;

namespace BR.MadenIlan.Auth.Mapping.AutoMapperProfile
{
    public class IntrospectProfile : Profile
    {
        public IntrospectProfile()
        {
            CreateMap<TokenIntrospectionResponse, Introspect>()
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.IsActive));
        }
    }
}
