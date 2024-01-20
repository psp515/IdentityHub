﻿using FluentValidation;

namespace ImageHub.Api.Features.Images.AddImage;

public class AddImageValidator : AbstractValidator<AddImageCommand>
{
    private static readonly string[] AllowedFileTypes = 
    { 
        "image/jpeg", 
        "image/png", 
        "image/svg" 
    };

    private static int MaxKiloBytes = 48;
    private static int BytesInKiloByte = 1024;

    public AddImageValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Image.Length)
            .LessThan(MaxKiloBytes * BytesInKiloByte);

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.Image)
            .NotNull();

        RuleFor(x => x.FileType)
            .Must(x => AllowedFileTypes.Contains(x))
            .WithMessage($"Invalid file type. Allowed types: {string.Join(',',AllowedFileTypes)}");
    }
}
