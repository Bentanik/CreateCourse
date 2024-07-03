using FluentValidation;
using Microsoft.AspNetCore.Http;
using Server.Contracts.Abstractions.Course;
using System.Linq;

namespace Server.Application.Validations;

public class CreateCourseRequestContentValidator : AbstractValidator<CreateCourseContentRequest>
{
    public CreateCourseRequestContentValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.CourseId).NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.");

        RuleFor(x => x.Video).NotNull().WithMessage("Video file is required.");
        RuleFor(x => x.Video).Must(BeAValidVideoFile).WithMessage("Video file must be a valid video file (mp4, mov, avi, mkv).");
        RuleFor(x => x.Video).Must(HaveValidSize).WithMessage("Video file size must be less than 100MB.");
    }

    private bool BeAValidVideoFile(IFormFile file)
    {
        if (file == null)
            return false;

        var allowedExtensions = new[] { ".mp4", ".mov", ".avi", ".mkv" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return allowedExtensions.Contains(fileExtension);
    }

    private bool HaveValidSize(IFormFile file)
    {
        if (file == null)
            return false;

        const int maxFileSize = 100 * 1024 * 1024;
        return file.Length <= maxFileSize;
    }
}
