using ChatServer.Domain.Entities;

namespace ChatServer.Domain.Abstract;

public interface IUserRepository
{
    Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);
}
