namespace DocumentService.Application.Common.Results;

public sealed record Error
{
    public string Code { get; }
    public string Message { get; }
    public ErrorCode Category { get; }

    private Error(string code, string message, ErrorCode category)
    {
        Code = code;
        Message = message;
        Category = category;
    }

    public static Error Create(string code, string message, ErrorCode category)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Code required", nameof(code));
        }
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Message required", nameof(message));
        }
        return new Error(code.Trim(), message.Trim(), category);
    }

    public static Error Validation(string code, string message) => Create(code, message, ErrorCode.Validation);
    public static Error NotFound(string code, string message) => Create(code, message, ErrorCode.NotFound);
    public static Error Conflict(string code, string message) => Create(code, message, ErrorCode.Conflict);
    public static Error AlreadyExists(string code, string message) => Create(code, message, ErrorCode.AlreadyExists);
    public static Error Unexpected(string code, string message) => Create(code, message, ErrorCode.Unexpected);
}
