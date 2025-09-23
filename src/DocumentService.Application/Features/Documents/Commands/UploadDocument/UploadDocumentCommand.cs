using DocumentService.Application.Common.Results;
using DocumentService.Domain.ValueObjects;
using MediatR;

namespace DocumentService.Application.Features.Documents.Commands.UploadDocument;

public sealed class UploadDocumentCommand : IRequest<Result<DocumentId>>
{
    public string FileName { get; }
    public string ContentType { get; }
    public long SizeBytes { get; }
    public Stream Content { get; }
    public string? Title { get; }
    public IReadOnlyCollection<string>? Tags { get; }

    public UploadDocumentCommand(string fileName, string contentType, long sizeBytes, Stream content, string? title, IReadOnlyCollection<string>? tags)
    {
        FileName = fileName;
        ContentType = contentType;
        SizeBytes = sizeBytes;
        Content = content;
        Title = title;
        Tags = tags;
    }
}
