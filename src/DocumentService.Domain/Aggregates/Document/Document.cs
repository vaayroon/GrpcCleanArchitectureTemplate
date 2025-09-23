using DocumentService.Domain.Abstractions;
using DocumentService.Domain.Events;
using DocumentService.Domain.ValueObjects;

namespace DocumentService.Domain.Aggregates.Document;

public sealed class Document : AggregateRoot<DocumentId>
{
    private readonly List<string> _tags = new();

    public FileName FileName { get; private set; }
    public ContentType ContentType { get; private set; }
    public long SizeBytes { get; private set; }
    public Checksum Checksum { get; private set; }
    public StoragePath StoragePath { get; private set; }
    public string? Title { get; private set; }
    public IReadOnlyCollection<string> Tags => _tags.AsReadOnly();
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }
    public bool Deleted { get; private set; }

    private Document(DocumentId id,
        FileName fileName,
        ContentType contentType,
        long sizeBytes,
        Checksum checksum,
        StoragePath storagePath,
        string? title,
        IEnumerable<string>? tags)
    {
        Id = id;
        FileName = fileName;
        ContentType = contentType;
        SizeBytes = sizeBytes;
        Checksum = checksum;
        StoragePath = storagePath;
        Title = title;
        if (tags != null)
        {
            _tags.AddRange(tags.Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Trim()).Distinct());
        }

        CreatedAtUtc = UpdatedAtUtc = DateTime.UtcNow;
        Raise(new DocumentUploaded(Id, FileName, ContentType, SizeBytes, Checksum));
    }

    public static Document CreateNew(FileName fileName, ContentType contentType, long sizeBytes, Checksum checksum, StoragePath storagePath, string? title, IEnumerable<string>? tags)
    {
        if (sizeBytes <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sizeBytes));
        }

        return new Document(DocumentId.New(), fileName, contentType, sizeBytes, checksum, storagePath, title, tags);
    }

    public void UpdateMetadata(string? title, IEnumerable<string>? tags, ContentType? newContentType)
    {
        if (Deleted)
        {
            throw new InvalidOperationException("Cannot modify deleted document");
        }

        bool changed = false;
        if (title != null && title != Title)
        {
            Title = string.IsNullOrWhiteSpace(title) ? null : title.Trim();
            changed = true;
        }
        if (tags != null)
        {
            var newTags = tags.Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Trim()).Distinct().ToList();
            if (!newTags.SequenceEqual(_tags))
            {
                _tags.Clear();
                _tags.AddRange(newTags);
                changed = true;
            }
        }
        if (newContentType != null && newContentType.Value != ContentType.Value)
        {
            ContentType = newContentType;
            changed = true;
        }
        if (changed)
        {
            UpdatedAtUtc = DateTime.UtcNow;
            Raise(new DocumentMetadataUpdated(Id, Title, _tags.AsReadOnly(), newContentType));
        }
    }

    public void MarkDeleted()
    {
        if (Deleted)
        {
            return;
        }

        Deleted = true;
        UpdatedAtUtc = DateTime.UtcNow;
        Raise(new DocumentDeleted(Id));
    }
}
