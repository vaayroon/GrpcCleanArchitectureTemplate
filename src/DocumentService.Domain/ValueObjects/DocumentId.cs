using DocumentService.Domain.Abstractions;

namespace DocumentService.Domain.ValueObjects;

public sealed class DocumentId : ValueObject
{
    public Guid Value { get; }
    private DocumentId(Guid value) => Value = value;
    public static DocumentId New() => new(Guid.NewGuid());
    public static DocumentId From(Guid value) => new(value);
    protected override IEnumerable<object?> GetAtomicValues() { yield return Value; }
    public override string ToString() => Value.ToString();
}
