using Common.Core.Abstract;

namespace Flightware.Domain.DomainEvents;

public record UserCreatedDomainEvent : IDomainEvent
{
    public Guid Id { get; init; }
    public string Username { get; init; }
    public string Email { get; init; }
}
