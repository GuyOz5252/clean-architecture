using Flightware.Domain.Entities;

namespace Flightware.Domain.Abstract;

public interface IUserRepository
{
    Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);
}
