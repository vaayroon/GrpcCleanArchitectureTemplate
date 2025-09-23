using DocumentService.Domain.Aggregates.Document;
using DocumentService.Domain.ValueObjects;

namespace DocumentService.Application.Abstractions.Persistence;

public interface IDocumentRepository
{
    Task AddAsync(Document entity, CancellationToken cancellationToken);
    Task<Document?> GetByIdAsync(DocumentId id, CancellationToken cancellationToken);
    Task<Document?> GetByFileNameAsync(string normalizedFileName, CancellationToken cancellationToken);
    Task<IReadOnlyList<Document>> ListByFolderAsync(Guid? folderId, int pageSize, string? pageToken, CancellationToken cancellationToken);
}
