using FluentValidation;

namespace DocumentService.Application.Features.Documents.Commands.UploadDocument;

public sealed class UploadDocumentCommandValidator : AbstractValidator<UploadDocumentCommand>
{
    public UploadDocumentCommandValidator()
    {
        RuleFor(x => x.FileName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.ContentType).NotEmpty();
        RuleFor(x => x.SizeBytes).GreaterThan(0).LessThanOrEqualTo(1024L * 1024L * 1024L); // <=1GB example
        RuleFor(x => x.Content).NotNull();
    }
}
