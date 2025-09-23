using System.Security.Cryptography;
using System.Text;
using DocumentService.Domain.Abstractions;

namespace DocumentService.Domain.ValueObjects;

public sealed class Checksum : ValueObject
{
    public string Algorithm { get; }
    public string Value { get; }
    private Checksum(string algorithm, string value)
    {
        Algorithm = algorithm;
        Value = value;
    }
    public static Checksum FromBytes(ReadOnlySpan<byte> data)
    {
        using var sha256 = SHA256.Create();
        byte[] hash = sha256.ComputeHash(data.ToArray());
        return new Checksum("SHA256", Convert.ToHexString(hash));
    }
    public static Checksum FromString(string algorithm, string value) => new(algorithm, value);
    protected override IEnumerable<object?> GetAtomicValues() { yield return Algorithm; yield return Value; }
    public override string ToString() => $"{Algorithm}:{Value}";
}
