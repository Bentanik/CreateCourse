using AutoMapper;
using Server.Contracts.Abstractions.Course;
using Server.Contracts.DTOs.Course;

namespace Server.Application.Mappers.Course;

public class CourseProfile : Profile
{
   public CourseProfile()
    {
        CreateMap<CreateCourseRequest, CreateCourseDTO>();
        CreateMap<CreateCourseContentRequest, CreateCourseContentDTO>();
    }
}
