﻿using ImageHub.Api.Contracts.Image.AddImage;

namespace ImageHub.Api.Features.Images.AddImage;

public class AddImageCommand : IRequest<Result<AddImageResponse>>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;
    public Guid? GroupId { get; set; }
    public required IFormFile Image { get; set; }
}
