using Ardalis.Result;
using ChatServer.Core.Entities;

namespace ChatServer.Core.Abstract;

public interface IUserRepository
{
    Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);
    Task<Result<User>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}
