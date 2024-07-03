using Server.Application;
using Server.Application.Repositories;

namespace Server.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    private readonly IUserRepository _userRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ICourseContentRepository _courseContentRepository;
    public UnitOfWork(AppDbContext dbContext, IUserRepository userRepository, ICourseRepository courseRepository, ICourseContentRepository courseContentRepository)
    {
        _dbContext = dbContext;
        _userRepository = userRepository;
        _courseRepository = courseRepository;
        _courseContentRepository = courseContentRepository;
    }

    public IUserRepository userRepository => _userRepository;

    public ICourseRepository courseRepository => _courseRepository;

    public ICourseContentRepository courseContentRepository => _courseContentRepository;

    public async Task<int> SaveChangeAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

}
