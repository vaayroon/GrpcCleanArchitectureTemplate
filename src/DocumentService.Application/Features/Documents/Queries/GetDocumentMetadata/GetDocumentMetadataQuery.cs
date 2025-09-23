using DocumentService.Application.Common.Results;
using DocumentService.Domain.ValueObjects;
using MediatR;

namespace DocumentService.Application.Features.Documents.Queries.GetDocumentMetadata;

public sealed class GetDocumentMetadataQuery : IRequest<Result<DocumentMetadataDto>>
{
    public DocumentId Id { get; }
    public GetDocumentMetadataQuery(DocumentId id)
    {
        Id = id;
    }
}

public sealed class DocumentMetadataDto
{
    public Guid Id { get; init; }
    public string FileName { get; init; } = string.Empty;
    public string ContentType { get; init; } = string.Empty;
    public long SizeBytes { get; init; }
    public string Checksum { get; init; } = string.Empty;
    public string StoragePath { get; init; } = string.Empty;
    public string? Title { get; init; }
    public IReadOnlyCollection<string> Tags { get; init; } = Array.Empty<string>();
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
}
