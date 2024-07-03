using Server.Application.Repositories;
using Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Server.Application.Interfaces;

namespace Server.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly AppDbContext _dbContext;
    public UserRepository(AppDbContext dbContext, ICurrentTime timeService) : base(dbContext, timeService)
    {
        _dbContext = dbContext;
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
    }
}
