using Microsoft.AspNetCore.Http;

namespace Server.Contracts.Abstractions.Course;

public class CreateCourseContentRequest
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public string Title { get; set; }
    public IFormFile Video { get; set; }
}
