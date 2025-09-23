using DocumentService.Domain.ValueObjects;

namespace DocumentService.Domain.Events;

public sealed record DocumentDeleted(DocumentId DocumentId) : DomainEvent;
