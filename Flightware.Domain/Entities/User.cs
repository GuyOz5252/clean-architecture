using Common.Core.Abstract;

namespace Flightware.Domain.Entities;

public class User : EntityBase, IAggregateRoot
{
    public string Username { get; init; }
    public string Email { get; init; }
}
