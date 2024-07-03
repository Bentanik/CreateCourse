using AutoMapper;
using Server.Contracts.DTOs.Authentication;
using Server.Domain.Entities;

namespace Server.Application.Mappers.Authentication;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserDTO, User>()
            .ForMember(dest => dest.Active, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.RoleCodeId, opt => opt.MapFrom(src => 2));

        CreateMap<User, LoginUserDTO>();
    }
}
