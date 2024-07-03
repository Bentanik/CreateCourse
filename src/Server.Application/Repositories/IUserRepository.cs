using Server.Domain.Entities;

namespace Server.Application.Repositories;
public interface IUserRepository : IGenericRepository<User>
{
    Task<User> GetUserByEmail(string email);
}
