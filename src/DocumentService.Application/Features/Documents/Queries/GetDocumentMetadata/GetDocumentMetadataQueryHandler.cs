using DocumentService.Application.Abstractions.Persistence;
using DocumentService.Application.Common.Results;
using DocumentService.Domain.Aggregates.Document;
using DocumentService.Domain.ValueObjects;
using MediatR;

namespace DocumentService.Application.Features.Documents.Queries.GetDocumentMetadata;

public sealed class GetDocumentMetadataQueryHandler : IRequestHandler<GetDocumentMetadataQuery, Result<DocumentMetadataDto>>
{
    private readonly IDocumentRepository _repository;

    public GetDocumentMetadataQueryHandler(IDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<DocumentMetadataDto>> Handle(GetDocumentMetadataQuery request, CancellationToken cancellationToken)
    {
        Document? entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return Result.Failure<DocumentMetadataDto>(Error.NotFound("Document.NotFound", "Document not found."));
        }

        var dto = new DocumentMetadataDto
        {
            Id = entity.Id.Value,
            FileName = entity.FileName.Value,
            ContentType = entity.ContentType.Value,
            SizeBytes = entity.SizeBytes,
            Checksum = entity.Checksum.ToString(),
            StoragePath = entity.StoragePath.Value,
            Title = entity.Title,
            Tags = entity.Tags.ToList(),
            CreatedAtUtc = entity.CreatedAtUtc,
            UpdatedAtUtc = entity.UpdatedAtUtc
        };
        return Result.Success(dto);
    }
}
