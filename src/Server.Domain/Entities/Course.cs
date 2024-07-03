namespace Server.Domain.Entities;

public class Course : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ThumbNail { get; set; }
    public string ThumbNailId { get; set; }

    public ICollection<CourseContent>? CourseContents { get; set; }
}
