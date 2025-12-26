using Ardalis.Result;
using ChatServer.Domain.Abstract;
using ChatServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Result<User>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FindAsync([id], cancellationToken);
        return user is null ? Result.NotFound() : Result.Success(user);
    }

    public async Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        return user is null ? Result.NotFound() : Result.Success(user);
    }
}
