using Flightware.Domain.Abstract;
using Flightware.Domain.Entities;

namespace ChatServer.Infrastructure.Repositories;

public class EfCoreUserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public EfCoreUserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Users.AddAsync(user, cancellationToken);
        return result.Entity;
    }
}
