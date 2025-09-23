namespace DocumentService.Application.Common.Results;

public sealed class PagedResult<T> : Result<IReadOnlyList<T>>
{
    public string? NextPageToken { get; }
    private PagedResult(bool isSuccess, IReadOnlyList<T>? items, string? nextToken, Error? error)
        : base(isSuccess, items, error) { NextPageToken = nextToken; }

    public static PagedResult<T> Success(IReadOnlyList<T> items, string? nextPageToken)
    {
        return new PagedResult<T>(true, items, nextPageToken, null);
    }
    public static new PagedResult<T> Failure(Error error)
    {
        return new PagedResult<T>(false, null, null, error);
    }
}
