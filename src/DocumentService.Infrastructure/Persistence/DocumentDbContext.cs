using DocumentService.Domain.Aggregates.Document;
using DocumentService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.Infrastructure.Persistence;

public sealed class DocumentDbContext : DbContext
{
    public DbSet<Document> Documents => Set<Document>();

    public DocumentDbContext(DbContextOptions<DocumentDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DocumentDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
