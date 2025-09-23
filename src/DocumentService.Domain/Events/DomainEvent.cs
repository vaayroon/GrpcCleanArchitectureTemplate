namespace DocumentService.Domain.Events;

public abstract record DomainEvent(Guid Id, DateTime OccurredOnUtc) 
{
    protected DomainEvent() : this(Guid.NewGuid(), DateTime.UtcNow) {}
}
