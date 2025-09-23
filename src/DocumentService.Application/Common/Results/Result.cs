using System.Diagnostics.CodeAnalysis;

namespace DocumentService.Application.Common.Results;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    protected Result(bool isSuccess, Error? error)
    {
        if (isSuccess && error is not null)
        {
            throw new ArgumentException("Successful result cannot have an error", nameof(error));
        }
        if (!isSuccess && error is null)
        {
            throw new ArgumentException("Failure result must have an error", nameof(error));
        }
        IsSuccess = isSuccess;
        Error = error;
    }

    // Success overloads
    public static Result Success()
    {
        return new Result(true, null);
    }
    public static Result<T> Success<T>(T value)
    {
        return Result<T>.Success(value);
    }
    // Failure overloads
    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }
    public static Result<T> Failure<T>(Error error)
    {
        return Result<T>.Failure(error);
    }
}

public class Result<T> : Result
{
    private readonly T? _value;
    public T Value => IsSuccess ? _value! : throw new InvalidOperationException("Cannot access Value of a failed result");

    protected Result(bool isSuccess, T? value, Error? error) : base(isSuccess, error)
    {
        _value = value;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, null);
    }

    public static new Result<T> Failure(Error error)
    {
        return new Result<T>(false, default, error);
    }

    public bool TryGetValue([NotNullWhen(true)] out T? value)
    {
        if (IsSuccess)
        {
            value = _value!;
            return true;
        }
        value = default;
        return false;
    }

    public static implicit operator Result<T>(T value)
    {
        return Success(value);
    }
}
