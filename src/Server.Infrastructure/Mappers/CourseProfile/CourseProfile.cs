using AutoMapper;
using Server.Contracts.DTOs.Course;
using Server.Domain.Entities;

namespace Server.Infrastructure.Mappers.CourseProfile;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<CreateCourseDTO, Course>()
            .ForMember(dest => dest.ThumbNail, opt => opt.MapFrom(src => src.ThumbNailUrl));

        CreateMap<CreateCourseContentDTO, CourseContent>()
            .ForMember(dest => dest.ContentTitle, opt => opt.MapFrom(src => src.Title));
    }
}
