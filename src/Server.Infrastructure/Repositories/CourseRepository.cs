using Server.Application.Interfaces;
using Server.Application.Repositories;
using Server.Domain.Entities;

namespace Server.Infrastructure.Repositories;

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    private readonly AppDbContext _dbContext;
    public CourseRepository(AppDbContext dbContext, ICurrentTime timeService) : base(dbContext, timeService)
    {
        _dbContext = dbContext;
    }
}
