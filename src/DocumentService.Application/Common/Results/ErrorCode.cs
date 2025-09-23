namespace DocumentService.Application.Common.Results;

public enum ErrorCode
{
    None = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3,
    Unauthorized = 4,
    Forbidden = 5,
    Unexpected = 6,
    Concurrency = 7,
    AlreadyExists = 8,
    TooLarge = 9,
    UnsupportedMediaType = 10
}
