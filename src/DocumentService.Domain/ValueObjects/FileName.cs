using System.Text.RegularExpressions;
using DocumentService.Domain.Abstractions;

namespace DocumentService.Domain.ValueObjects;

public sealed class FileName : ValueObject
{
    private static readonly Regex ValidPattern = new("^[a-zA-Z0-9_.-]{1,255}$", RegexOptions.Compiled);
    public string Value { get; }
    private FileName(string value) => Value = value;
    public static FileName Create(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            throw new ArgumentException("File name cannot be empty", nameof(raw));
        }

        string trimmed = raw.Trim();
        if (!ValidPattern.IsMatch(trimmed))
        {
            throw new ArgumentException("Invalid file name format", nameof(raw));
        }

        return new FileName(trimmed);
    }
    protected override IEnumerable<object?> GetAtomicValues() { yield return Value; }
    public override string ToString() => Value;
}
