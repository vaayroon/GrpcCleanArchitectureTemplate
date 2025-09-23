namespace DocumentService.Domain.Abstractions;

public abstract class ValueObject
{
    protected abstract IEnumerable<object?> GetAtomicValues();

    public override bool Equals(object? obj)
    {
        if (obj is not ValueObject other)
        {
            return false;
        }

        using IEnumerator<object?> thisValues = GetAtomicValues().GetEnumerator();
        using IEnumerator<object?> otherValues = other.GetAtomicValues().GetEnumerator();
        while (thisValues.MoveNext() && otherValues.MoveNext())
        {
            if (thisValues.Current is null ^ otherValues.Current is null)
            {
                return false;
            }

            if (thisValues.Current is not null && !thisValues.Current.Equals(otherValues.Current))
            {
                return false;
            }
        }
        return !thisValues.MoveNext() && !otherValues.MoveNext();
    }

    public override int GetHashCode() => GetAtomicValues()
        .Select(x => x?.GetHashCode() ?? 0)
        .Aggregate(0, HashCode.Combine);
}
