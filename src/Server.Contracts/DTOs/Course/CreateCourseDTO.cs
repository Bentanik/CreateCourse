using Microsoft.AspNetCore.Http;

namespace Server.Contracts.DTOs.Course;

public class CreateCourseDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile ThumbNail { get; set; }
    public string? ThumbNailUrl { get; set; }
    public string? ThumbNailId { get; set; }

}
