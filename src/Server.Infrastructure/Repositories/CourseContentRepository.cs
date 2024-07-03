using Server.Application.Interfaces;
using Server.Application.Repositories;
using Server.Domain.Entities;

namespace Server.Infrastructure.Repositories;

public class CourseContentRepository : GenericRepository<CourseContent>, ICourseContentRepository
{
    private readonly AppDbContext _dbContext;
    public CourseContentRepository(AppDbContext dbContext, ICurrentTime timeService) : base(dbContext, timeService)
    {
        _dbContext = dbContext;
    }
}
