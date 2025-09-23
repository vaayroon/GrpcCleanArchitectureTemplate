using DocumentService.Domain.Abstractions;

namespace DocumentService.Domain.ValueObjects;

public sealed class StoragePath : ValueObject
{
    public string Value { get; }
    private StoragePath(string value) => Value = value;
    public static StoragePath Create(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            throw new ArgumentException("Storage path required", nameof(raw));
        }

        string normalized = raw.Replace("\\", "/", StringComparison.Ordinal).Trim();
        if (normalized.StartsWith('/'))
        {
            normalized = normalized[1..];
        }

        return new StoragePath(normalized);
    }
    protected override IEnumerable<object?> GetAtomicValues() { yield return Value; }
    public override string ToString() => Value;
}
