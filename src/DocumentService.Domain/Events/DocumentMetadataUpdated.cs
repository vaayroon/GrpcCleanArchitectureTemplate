using DocumentService.Domain.ValueObjects;

namespace DocumentService.Domain.Events;

public sealed record DocumentMetadataUpdated(DocumentId DocumentId, string? Title, IReadOnlyCollection<string> Tags, ContentType? NewContentType) : DomainEvent;
