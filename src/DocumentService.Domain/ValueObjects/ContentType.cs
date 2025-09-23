using DocumentService.Domain.Abstractions;

namespace DocumentService.Domain.ValueObjects;

public sealed class ContentType : ValueObject
{
    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase)
    {
        "application/pdf",
        "image/png",
        "image/jpeg",
        "text/plain"
    };

    public string Value { get; }
    private ContentType(string value) => Value = value;
    public static ContentType Create(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            throw new ArgumentException("ContentType required", nameof(raw));
        }

        string v = raw.Trim().ToLowerInvariant();
        if (!Allowed.Contains(v))
        {
            throw new ArgumentException($"ContentType '{v}' not allowed", nameof(raw));
        }

        return new ContentType(v);
    }
    protected override IEnumerable<object?> GetAtomicValues() { yield return Value; }
    public override string ToString() => Value;
}
