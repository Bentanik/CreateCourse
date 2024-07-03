using Server.Contracts.Abstractions.Course;
using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTOs.Course;

namespace Server.Application.Interfaces;

public interface ICourseService
{
    Task<Result<object>> UploadCourse(CreateCourseDTO courseDto);
    Task<Result<object>> UploadCourseContent(CreateCourseContentDTO courseContentDto);

}
