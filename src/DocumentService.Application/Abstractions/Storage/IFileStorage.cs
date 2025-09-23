using DocumentService.Domain.ValueObjects;

namespace DocumentService.Application.Abstractions.Storage;

public interface IFileStorage
{
    Task<StoragePath> StoreAsync(Stream content, FileName fileName, ContentType contentType, CancellationToken cancellationToken);
    Task<Stream?> RetrieveAsync(StoragePath path, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(StoragePath path, CancellationToken cancellationToken);
}
