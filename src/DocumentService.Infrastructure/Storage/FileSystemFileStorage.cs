using DocumentService.Application.Abstractions.Storage;
using DocumentService.Domain.ValueObjects;
using DocumentService.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DocumentService.Infrastructure.Storage;

public sealed class FileSystemFileStorage : IFileStorage
{
    private readonly StorageOptions _options;
    private readonly ILogger<FileSystemFileStorage> _logger;

    public FileSystemFileStorage(IOptions<StorageOptions> options, ILogger<FileSystemFileStorage> logger)
    {
        _options = options.Value;
        _logger = logger;
        Directory.CreateDirectory(_options.LocalRootPath);
    }

    public async Task<StoragePath> StoreAsync(Stream content, FileName fileName, ContentType contentType, CancellationToken cancellationToken)
    {
        string relative = $"{DateTime.UtcNow:yyyy/MM/dd}/{Guid.NewGuid()}_{fileName.Value}";
        string fullPath = Path.Combine(_options.LocalRootPath, relative);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
        await using var fs = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 64 * 1024, useAsync: true);
        content.Position = 0;
        await content.CopyToAsync(fs, cancellationToken);
        _logger.LogInformation("Stored file {FileName} at {Path}", fileName.Value, relative);
        return StoragePath.Create(relative);
    }

    public Task<Stream?> RetrieveAsync(StoragePath path, CancellationToken cancellationToken)
    {
        string fullPath = Path.Combine(_options.LocalRootPath, path.Value);
        if (!File.Exists(fullPath))
        {
            return Task.FromResult<Stream?>(null);
        }
        Stream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read, 64 * 1024, useAsync: true);
        return Task.FromResult<Stream?>(stream);
    }

    public Task<bool> DeleteAsync(StoragePath path, CancellationToken cancellationToken)
    {
        string fullPath = Path.Combine(_options.LocalRootPath, path.Value);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}
