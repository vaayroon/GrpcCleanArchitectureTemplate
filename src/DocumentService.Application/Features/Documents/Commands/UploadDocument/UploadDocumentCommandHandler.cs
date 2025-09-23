using DocumentService.Application.Abstractions.Persistence;
using DocumentService.Application.Abstractions.Storage;
using DocumentService.Application.Common.Results;
using DocumentService.Domain.Aggregates.Document;
using DocumentService.Domain.ValueObjects;
using MediatR;

namespace DocumentService.Application.Features.Documents.Commands.UploadDocument;

public sealed class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, Result<DocumentId>>
{
    private readonly IDocumentRepository _repository;
    private readonly IFileStorage _fileStorage;
    private readonly IUnitOfWork _unitOfWork;

    public UploadDocumentCommandHandler(IDocumentRepository repository, IFileStorage fileStorage, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _fileStorage = fileStorage;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DocumentId>> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var fileName = FileName.Create(request.FileName);
            ContentType contentType;
            try
            {
                contentType = ContentType.Create(request.ContentType);
            }
            catch (ArgumentException ex)
            {
                return Result.Failure<DocumentId>(Error.Validation("ContentType.Invalid", ex.Message));
            }

            // Check duplicate by file name (simplistic scenario)
            Document? existing = await _repository.GetByFileNameAsync(fileName.Value, cancellationToken);
            if (existing != null)
            {
                return Result.Failure<DocumentId>(Error.Conflict("Document.Duplicate", "A document with the same name already exists."));
            }

            // Compute checksum and store content
            using var ms = new MemoryStream();
            await request.Content.CopyToAsync(ms, cancellationToken);
            byte[] bytes = ms.ToArray();
            var checksum = Checksum.FromBytes(bytes);
            ms.Position = 0;
            StoragePath storagePath = await _fileStorage.StoreAsync(ms, fileName, contentType, cancellationToken);

            var document = Document.CreateNew(fileName, contentType, request.SizeBytes, checksum, storagePath, request.Title, request.Tags);
            await _repository.AddAsync(document, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success(document.Id);
        }
        catch (Exception ex)
        {
            return Result.Failure<DocumentId>(Error.Unexpected("UploadDocument.Unhandled", ex.Message));
        }
    }
}
