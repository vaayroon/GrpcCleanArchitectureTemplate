using DocumentService.Domain.Aggregates.Document;
using DocumentService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentService.Infrastructure.Persistence.Configurations;

public sealed class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("documents");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasConversion(id => id.Value, v => DocumentId.From(v));

        builder.Property(d => d.FileName)
            .HasConversion(fn => fn.Value, v => FileName.Create(v))
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(d => d.ContentType)
            .HasConversion(ct => ct.Value, v => ContentType.Create(v))
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(d => d.Checksum)
            .HasConversion(
                c => c.ToString(),
                v => Checksum.FromString("SHA256", v)
            )
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(d => d.StoragePath)
            .HasConversion(p => p.Value, v => StoragePath.Create(v))
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(d => d.Title)
            .HasMaxLength(512);

        builder.Property(d => d.SizeBytes)
            .IsRequired();

        builder.Property(d => d.CreatedAtUtc).IsRequired();
        builder.Property(d => d.UpdatedAtUtc).IsRequired();
        builder.Property(d => d.Deleted).IsRequired();

        builder.Ignore(d => d.DomainEvents);

        builder.OwnsMany<string>("_tags", tags =>
        {
            tags.ToTable("document_tags");
            tags.WithOwner().HasForeignKey("DocumentId");
            tags.Property<int>("Id");
            tags.HasKey("Id");
            tags.Property(t => t)
                .HasColumnName("Tag")
                .HasMaxLength(128)
                .IsRequired();
            tags.HasIndex("DocumentId", "Tag").IsUnique();
        });

        builder.HasIndex(d => d.FileName).IsUnique();
    }
}
