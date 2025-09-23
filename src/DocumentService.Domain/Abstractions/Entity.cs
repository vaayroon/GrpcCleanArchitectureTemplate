using DocumentService.Domain.Events;

namespace DocumentService.Domain.Abstractions;

public abstract class Entity<TId>
    where TId : notnull
{
    private readonly List<DomainEvent> _domainEvents = new();
    public TId Id { get; protected set; } = default!;

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void Raise(DomainEvent @event) => _domainEvents.Add(@event);
    public void ClearDomainEvents() => _domainEvents.Clear();
}
