using Microsoft.AspNetCore.Http;

namespace Server.Contracts.Abstractions.Course;

public class CreateCourseRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile ThumbNail { get; set; }
}
