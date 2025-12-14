namespace ChatServer.Domain.Abstract;

public abstract class EntityBase
{
  private readonly List<IDomainEvent> _domainEvents = [];

  public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

  public Guid Id { get; init; }

  public void ClearDomainEvents() => _domainEvents.Clear();

  public void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
