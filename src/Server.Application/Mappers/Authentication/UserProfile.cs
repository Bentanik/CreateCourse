using AutoMapper;
using Server.Contracts.Abstractions.Authentication;
using Server.Contracts.DTOs.Authentication;

namespace Server.Application.Mappers.Authentication;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserRequest, CreateUserDTO>();
        CreateMap<CreateUserDTO, CreateUserRequest>();

        CreateMap<LoginRequest, LoginUserDTO>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "user"));
    }
}
