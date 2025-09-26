using DocumentService.Application.Abstractions.Persistence;
using DocumentService.Domain.Aggregates.Document;
using DocumentService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.Infrastructure.Persistence.Repositories;

public sealed class DocumentRepository : IDocumentRepository
{
    private readonly DocumentDbContext _context;

    public DocumentRepository(DocumentDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Document entity, CancellationToken cancellationToken)
    {
        await _context.Documents.AddAsync(entity, cancellationToken);
    }

    public async Task<Document?> GetByIdAsync(DocumentId id, CancellationToken cancellationToken)
    {
        return await _context.Documents.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<Document?> GetByFileNameAsync(string normalizedFileName, CancellationToken cancellationToken)
    {
        return await _context.Documents.FirstOrDefaultAsync(d => d.FileName.Value == normalizedFileName, cancellationToken);
    }

    public async Task<IReadOnlyList<Document>> ListByFolderAsync(Guid? folderId, int pageSize, string? pageToken, CancellationToken cancellationToken)
    {
        // Folder relationship not yet implemented; placeholder simple paging by CreatedAtUtc
        IQueryable<Document> query = _context.Documents.OrderByDescending(d => d.CreatedAtUtc);
        if (!string.IsNullOrEmpty(pageToken) && DateTime.TryParse(pageToken, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime cursor))
        {
            query = query.Where(d => d.CreatedAtUtc < cursor);
        }
        List<Document> items = await query.Take(pageSize).ToListAsync(cancellationToken);
        return items;
    }
}
