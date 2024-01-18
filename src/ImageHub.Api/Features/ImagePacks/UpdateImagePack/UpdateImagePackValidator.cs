﻿using FluentValidation;

namespace ImageHub.Api.Features.ImagePacks.AddImagePack;

public class UpdateImagePackValidator : AbstractValidator<UpdateImagePackCommand>
{
    public UpdateImagePackValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("Provided id is not valid id.");

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}