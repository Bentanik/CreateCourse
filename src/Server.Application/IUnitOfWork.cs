using Server.Application.Repositories;

namespace Server.Application;

public interface IUnitOfWork
{
    Task<int> SaveChangeAsync();
    IUserRepository userRepository { get;}
    ICourseRepository courseRepository { get;}
    ICourseContentRepository courseContentRepository { get;}
}
