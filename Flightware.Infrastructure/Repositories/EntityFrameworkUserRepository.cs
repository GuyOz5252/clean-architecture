using Flightware.Domain.Abstract;
using Flightware.Domain.Entities;

namespace Flightware.Infrastructure.Repositories;

public class EntityFrameworkUserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public EntityFrameworkUserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Users.AddAsync(user, cancellationToken);
        return result.Entity;
    }
}
