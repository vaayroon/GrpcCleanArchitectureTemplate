using DocumentService.Domain.ValueObjects;

namespace DocumentService.Domain.Events;

public sealed record DocumentUploaded(DocumentId DocumentId, FileName FileName, ContentType ContentType, long SizeBytes, Checksum Checksum) : DomainEvent;
