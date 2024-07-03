namespace Server.Domain.Entities;
public class CourseContent : BaseEntity
{
    public string ContentTitle { get; set; }
    public string ContentTime { get; set; }
    public string ContentUrl { get; set; }
    public string ContentId { get; set; }
    public Guid CourseId { get; set; }
    public Course Course { get; set; }
}
