using DocumentService.Domain.Abstractions;
using DocumentService.Domain.ValueObjects;

namespace DocumentService.Domain.Aggregates.Folder;

public sealed class Folder : AggregateRoot<Guid>
{
    private readonly List<DocumentId> _documentIds = new();
    private readonly List<Folder> _subFolders = new();

    public string Name { get; private set; }
    public Guid? ParentFolderId { get; private set; }
    public IReadOnlyCollection<DocumentId> Documents => _documentIds.AsReadOnly();
    public IReadOnlyCollection<Folder> SubFolders => _subFolders.AsReadOnly();

    private Folder(Guid id, string name, Guid? parent)
    {
        Id = id;
        Name = name;
        ParentFolderId = parent;
    }

    public static Folder CreateRoot(string name) => CreateInternal(name, null);
    public static Folder CreateChild(string name, Guid parentId) => CreateInternal(name, parentId);

    private static Folder CreateInternal(string name, Guid? parent)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Folder name required", nameof(name));
        }

        return new Folder(Guid.NewGuid(), name.Trim(), parent);
    }

    public void AddDocument(DocumentId id)
    {
        if (!_documentIds.Contains(id))
        {
            _documentIds.Add(id);
        }
    }

    public void RemoveDocument(DocumentId id)
    {
        _documentIds.Remove(id);
    }

    public Folder AddSubFolder(string name)
    {
        Folder folder = CreateChild(name, Id);
        _subFolders.Add(folder);
        return folder;
    }
}
