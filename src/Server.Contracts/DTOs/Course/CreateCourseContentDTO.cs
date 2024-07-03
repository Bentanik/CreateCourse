using Microsoft.AspNetCore.Http;

namespace Server.Contracts.DTOs.Course;

public class CreateCourseContentDTO
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public string Title { get; set; }
    public IFormFile Video { get; set; }
    public string? ContentUrl { get; set; }
    public string? ContentId { get; set; }
    public string? ContentTime { get; set; }
}
